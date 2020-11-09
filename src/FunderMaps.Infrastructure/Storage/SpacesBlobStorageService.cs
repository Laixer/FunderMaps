using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Infrastructure.Storage
{
    /// <summary>
    ///     Amazon S3 implementation of <see cref="IBlobStorageService"/>.
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
        public SpacesBlobStorageService(IOptions<BlobStorageOptions> options, ILogger<SpacesBlobStorageService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            client = new AmazonS3Client(new BasicAWSCredentials(_options.AccessKey, _options.SecretKey),
                new AmazonS3Config
                {
                    ServiceURL = _options.ServiceUri.AbsoluteUri
                });
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
        public async Task<bool> FileExistsAsync(string containerName, string fileName)
        {
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
        public Task<Uri> GetAccessLinkAsync(string containerName, string fileName, double hoursValid)
        {
            try
            {
                var url = client.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = _options.BlobStorageName,
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}",
                    Expires = DateTime.UtcNow.AddHours(hoursValid)
                });

                return Task.FromResult(new Uri(url));
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
        public async Task StoreFileAsync(string containerName, string fileName, Stream stream)
        {
            try
            {
                var key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}";
                using var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(stream, _options.BlobStorageName, key);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, "Could not store file to Spaces using S3");

                throw new StorageException("Could not store file", e);
            }
        }

        // FUTURE: Refactor
        /// <summary>
        ///     Stores a file in a Digital Ocean Space.
        /// </summary>
        /// <param name="containerName">The container name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="stream">See <see cref="Stream"/>.</param>
        /// <param name="storageObject">Storage object settings.</param>
        /// <returns>See <see cref="ValueTask"/>.</returns>
        public async Task StoreFileAsync(string containerName, string fileName, string contentType, Stream stream, StorageObject storageObject)
        {
            try
            {
                var request = new TransferUtilityUploadRequest
                {
                    BucketName = _options.BlobStorageName,
                    ContentType = contentType,
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}",
                    InputStream = stream,
                };

                if (storageObject != null)
                {
                    request.CannedACL = storageObject.IsPublic ? S3CannedACL.PublicRead : S3CannedACL.Private;
                    request.Headers.ContentType = storageObject.ContentType ?? request.Headers.ContentType;
                    request.Headers.CacheControl = storageObject.CacheControl ?? request.Headers.CacheControl;
                    request.Headers.ContentDisposition = storageObject.ContentDisposition ?? request.Headers.ContentDisposition;
                    request.Headers.ContentEncoding = storageObject.ContentEncoding ?? request.Headers.ContentEncoding;
                }

                using var transferUtility = new TransferUtility(client);
                await transferUtility.UploadAsync(request);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, $"Could not store file with content type {contentType} to Spaces using S3");

                throw new StorageException($"Could not upload file with content type {contentType}", e);
            }
        }

        // FUTURE: Refactor
        /// <summary>
        ///     Stores a file in a Digital Ocean Space.
        /// </summary>
        /// <param name="directoryName">Directory name at the destination including prefix paths.</param>
        /// <param name="directoryPath">Source directory.</param>
        /// <param name="storageObject">Storage object settings.</param>
        /// <returns>See <see cref="ValueTask"/>.</returns>
        public async Task StoreDirectoryAsync(string directoryName, string directoryPath, StorageObject storageObject)
        {
            try
            {
                async Task DirectorySearch(string path, string subdir = "")
                {
                    foreach (var file in Directory.GetFiles(path))
                    {
                        using Stream stream = File.OpenRead(file);
                        await StoreFileAsync(directoryName, Path.Combine(subdir, Path.GetFileName(file)), storageObject.ContentType, stream, storageObject);
                    }
                    foreach (var file in Directory.GetDirectories(path))
                    {
                        await DirectorySearch(file, Path.Combine(subdir, Path.GetFileName(file)));
                    }
                }

                await DirectorySearch(directoryPath);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e, "Could not store file to Spaces using S3");

                throw new StorageException("Could not store file", e);
            }
        }

        // FUTURE: From interface
        /// <summary>
        ///     Test the Amazon S3 service backend.
        /// </summary>
        public async Task TestService()
            => await client.ListBucketsAsync();
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
