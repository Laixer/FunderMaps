using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
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

            options.Value.AccesKey.ThrowIfNullOrEmpty();
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
            var credentials = new BasicAWSCredentials(_options.AccesKey, _options.SecretKey);
            client = new AmazonS3Client(credentials, config);
        }

        /// <summary>
        ///     Called on graceful shutdown.
        /// </summary>
        public void Dispose() => client.Dispose();

        // TODO Amazon has no clean way to check for object existence.
        /// <summary>
        ///     Checks if a file exists or not.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>Boolean result.</returns>
        public async ValueTask<bool> FileExistsAsync(string containerName, string fileName)
        {
            fileName.ThrowIfNullOrEmpty();

            try
            {
                // TODO Maybe use list keys with a filter?

                var result = await client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = _options.BlobStorageName,
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}"
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
        /// <param name="containerName">The container name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="hoursValid">How many hours the link should be valid.</param>
        /// <returns>Access <see cref="Uri"/>.</returns>
        public ValueTask<Uri> GetAccessLinkAsync(string containerName, string fileName, double hoursValid)
        {
            try
            {
                fileName.ThrowIfNullOrEmpty();
                if (hoursValid <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(hoursValid));
                }

                var uri = new Uri(client.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = _options.BlobStorageName,
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}",
                    Expires = DateTime.UtcNow.AddHours(hoursValid)
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

                var key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}";
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
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}",
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
    }
}
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
