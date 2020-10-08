using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
namespace FunderMaps.Infrastructure.Storage
{
    /// <summary>
    ///     Digital Ocean Spaces implementation of <see cref="IBlobStorageService"/>.
    /// </summary>
    /// <remarks>
    ///     This creates an <see cref="IAmazonS3"/> client once in its constructor.
    ///     Register this service as a singleton if dependency injection is used.
    /// </remarks>
    internal class SpacesBlobStorageService : IBlobStorageService, IDisposable
    {
        private readonly BlobStorageOptions _options;
        private readonly IAmazonS3 client;
        private readonly ILogger<SpacesBlobStorageService> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public SpacesBlobStorageService(IOptions<BlobStorageOptions> options,
            ILogger<SpacesBlobStorageService> logger)
        {
            if (options == null || options.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Value.AccessKey.ThrowIfNullOrEmpty();
            options.Value.SecretKey.ThrowIfNullOrEmpty();

            if (options.Value.ServiceUri == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Create client once.
            var config = new AmazonS3Config
            {
                ServiceURL = _options.ServiceUri.AbsoluteUri
            };
            var credentials = new BasicAWSCredentials(_options.AccessKey, _options.SecretKey);
            client = new AmazonS3Client(credentials, config);
        }

        /// <summary>
        ///     Called on graceful shutdown.
        /// </summary>
        public void Dispose() => client.Dispose();

        /// <summary>
        ///     Lists all names of subdirectories in a given container.
        /// </summary>
        /// <remarks>
        ///     All container names are returned without trailing slash.
        /// </remarks>
        /// <param name="containerName">The container to check.</param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<string>> ListSubcontainerNamesAsync(string containerName)
        {
            containerName.ThrowIfNullOrEmpty();

            try
            {
                // Note: using a delimiter of '/' is used to group results. TODO Understand.
                var result = await client.ListObjectsAsync(new ListObjectsRequest
                {
                    BucketName = _options.BlobStorageName,
                    Prefix = WithTrailingSlash(containerName),
                    Delimiter = "/"
                });

                var basePath = $"{WithoutTrailingSlash(containerName)}/";
                return result.CommonPrefixes.Select(x => WithoutTrailingSlash(x.Replace(basePath, "", StringComparison.InvariantCulture)));
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, "Could not list subdirectories in spaces using S3");
                throw new StorageException("Could not list subdirectories", e);
            }
        }

        // TODO Amazon has no clean way to check for object existence.
        /// <summary>
        ///     Checks if a file exists or not.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>Boolean result.</returns>
        public async ValueTask<bool> FileExistsAsync(string containerName, string fileName)
        {
            // TODO Remove
            try
            {
                var obj = await client.ListObjectsAsync(new ListObjectsRequest
                {
                    BucketName = _options.BlobStorageName,
                    Prefix = "doesnotexist",
                    Delimiter = "/"
                });
            }
            catch (Exception)
            {
                throw;
            }
            // END remove

            fileName.ThrowIfNullOrEmpty();

            try
            {
                // TODO Maybe use list keys with a filter?

                var result = await client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = _options.BlobStorageName,
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{WithTrailingSlash(containerName)}{fileName}"
                });

                return true;
            }
            catch (Exception e)
            {
                // This type of exception indicates that the file does not exist.
                if (e is AmazonS3Exception && ((AmazonS3Exception)e).ErrorCode == "NoSuchKey")
                {
                    return false;
                }

                _logger.LogError(e, "Could not check file existence in Spaces using S3");
                // TODO QUESTION: Inner exception or not? I don't think so because we already log it.
                throw new StorageException("Could not check file existence", e);
            }
        }

        /// <summary>
        ///     Gets an access uri for a given file.
        /// </summary>
        /// <remarks>
        ///     The default <paramref name="accessType"/> is read.
        /// </remarks>
        /// <param name="containerName">The container name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="hoursValid">How many hours the link should be valid.</param>
        /// <param name="accessType">Indicates what we want to do with the link.</param>
        /// <returns>Access <see cref="Uri"/>.</returns>
        public ValueTask<Uri> GetAccessLinkAsync(string containerName, string fileName, double hoursValid, AccessType accessType = AccessType.Read)
        {
            fileName.ThrowIfNullOrEmpty();
            if (hoursValid <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hoursValid));
            }

            try
            {
                var uri = new Uri(client.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = _options.BlobStorageName,
                    //Key = string.IsNullOrEmpty(containerName) ? fileName : $"{WithTrailingSlash(containerName)}{fileName}",
                    Expires = DateTime.UtcNow.AddHours(hoursValid),
                    Verb = accessType switch
                    {
                        AccessType.Read => HttpVerb.GET,
                        AccessType.Write => HttpVerb.PUT,
                        _ => throw new InvalidOperationException(nameof(accessType))
                    }
                }));

                return new ValueTask<Uri>(uri);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, "Could not get access link from Spaces using S3");
                throw new StorageException("Could not get access link", e);
            }
        }

        /// <summary>
        ///     Stores a file in a Digital Ocean Space.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="stream">See <see cref="Stream"/>.</param>
        /// <returns>See <see cref="ValueTask"/>.</returns>
        public ValueTask StoreFileAsync(string containerName, string fileName, Stream stream)
        {
            try
            {
                fileName.ThrowIfNullOrEmpty();
                if (stream == null)
                {
                    throw new ArgumentNullException(nameof(stream));
                }

                var key = string.IsNullOrEmpty(containerName) ? fileName : $"{WithTrailingSlash(containerName)}{fileName}";
                using var transferUtility = new TransferUtility(client);

                return new ValueTask(transferUtility.UploadAsync(stream, _options.BlobStorageName, key));
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, "Could not store file to Spaces using S3");
                throw new StorageException("Could not store file", e);
            }
        }

        /// <summary>
        ///     Stores a file in a Digital Ocean Space.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="stream">See <see cref="Stream"/>.</param>
        /// <returns>See <see cref="ValueTask"/>.</returns>
        public ValueTask StoreFileAsync(string containerName, string fileName, string contentType, Stream stream)
        {
            fileName.ThrowIfNullOrEmpty();
            contentType.ThrowIfNullOrEmpty();
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            try
            {
                using var transferUtility = new TransferUtility(client);
                var request = new TransferUtilityUploadRequest
                {
                    BucketName = _options.BlobStorageName,
                    ContentType = contentType,
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{WithTrailingSlash(containerName)}{fileName}",
                    InputStream = stream
                };

                return new ValueTask(transferUtility.UploadAsync(request));
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, $"Could not store file with content type {contentType} to Spaces using S3");
                throw new StorageException($"Could not upload file with content type {contentType}", e);
            }
        }

        /// <summary>
        ///     Removes a / or \ from the end of a string if present.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <returns>The string without trailing slash.</returns>
        private static string WithoutTrailingSlash(string input)
        {
            if (input.EndsWith("\\", StringComparison.InvariantCulture))
            {
                return input[0..^1];
            }

            if (input.EndsWith("/", StringComparison.InvariantCulture))
            {
                return input[0..^1];
            }

            return input;
        }

        /// <summary>
        ///     Ensures a / at the end of a string.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <returns>The string with trailing slash.</returns>
        private static string WithTrailingSlash(string input)
            => $"{WithoutTrailingSlash(input)}/";
    }
}
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
