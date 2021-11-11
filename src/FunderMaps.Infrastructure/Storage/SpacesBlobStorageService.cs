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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Infrastructure.Storage
{
    /// <summary>
    ///     Amazon S3 implementation of <see cref="IBlobStorageService"/>.
    /// </summary>
    /// <remarks>
    ///     This creates an <see cref="IAmazonS3"/> client once in its constructor.
    ///     Register this service as a singleton if dependency injection is used.
    /// </remarks>
    internal class SpacesBlobStorageService : IBlobStorageService
    {
        private static readonly byte MaxKeys = 255;

        private readonly BlobStorageOptions _options;
        private readonly ILogger<SpacesBlobStorageService> _logger;
        private readonly AWSCredentials _awsCredentials;
        private readonly AmazonS3Config _clientConfig;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public SpacesBlobStorageService(IOptions<BlobStorageOptions> options, ILogger<SpacesBlobStorageService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _awsCredentials = new BasicAWSCredentials(_options.AccessKey, _options.SecretKey);
            _clientConfig = new()
            {
                ServiceURL = _options.ServiceUri.AbsoluteUri
            };
        }

        /// <summary>
        ///     Create a new Amazon S3 client.
        /// </summary>
        protected AmazonS3Client CreateClient => new(_awsCredentials, _clientConfig);

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
                var url = CreateClient.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = _options.BlobStorageName,
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}",
                    Expires = DateTime.UtcNow.AddHours(hoursValid)
                });

                return Task.FromResult(new Uri(url));
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError("Could not get access link from Spaces using S3");

                throw new StorageException("Could not get access link", e);
            }
        }

        /// <summary>
        ///     Stores a file.
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
                TransferUtilityUploadRequest request = new()
                {
                    BucketName = _options.BlobStorageName,
                    ContentType = contentType,
                    Key = string.IsNullOrEmpty(containerName) ? fileName : $"{containerName}/{fileName}",
                    InputStream = stream,
                };

                if (storageObject is not null)
                {
                    request.CannedACL = storageObject.IsPublic ? S3CannedACL.PublicRead : S3CannedACL.Private;
                    request.Headers.ContentType = storageObject.ContentType ?? request.Headers.ContentType;
                    request.Headers.CacheControl = storageObject.CacheControl ?? request.Headers.CacheControl;
                    request.Headers.ContentDisposition = storageObject.ContentDisposition ?? request.Headers.ContentDisposition;
                    request.Headers.ContentEncoding = storageObject.ContentEncoding ?? request.Headers.ContentEncoding;
                }

                using TransferUtility transferUtility = new(CreateClient);
                await transferUtility.UploadAsync(request);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError($"Could not store file with content type {contentType} to Spaces using S3");

                throw new StorageException($"Could not upload file with content type {contentType}", e);
            }
        }

        /// <summary>
        ///     Stores a directory.
        /// </summary>
        /// <param name="directoryName">Directory name at the destination including prefix paths.</param>
        /// <param name="directoryPath">Source directory.</param>
        /// <param name="storageObject">Storage object settings.</param>
        /// <returns>See <see cref="ValueTask"/>.</returns>
        public async Task StoreDirectoryAsync(string directoryName, string directoryPath, StorageObject storageObject)
        {
            try
            {
                TransferUtilityUploadDirectoryRequest request = new()
                {
                    BucketName = _options.BlobStorageName,
                    Directory = directoryPath,
                    KeyPrefix = directoryName,
                    SearchOption = SearchOption.AllDirectories,
                    UploadFilesConcurrently = true,
                };

                request.UploadDirectoryFileRequestEvent += (sender, uploadDirectoryRequest) =>
                {
                    uploadDirectoryRequest.UploadRequest.CannedACL = (storageObject?.IsPublic ?? false) ? S3CannedACL.PublicRead : S3CannedACL.Private;
                    uploadDirectoryRequest.UploadRequest.Headers.ContentType = storageObject?.ContentType ?? uploadDirectoryRequest.UploadRequest.Headers.ContentType;
                    uploadDirectoryRequest.UploadRequest.Headers.CacheControl = storageObject?.CacheControl ?? uploadDirectoryRequest.UploadRequest.Headers.CacheControl;
                    uploadDirectoryRequest.UploadRequest.Headers.ContentDisposition = storageObject?.ContentDisposition ?? uploadDirectoryRequest.UploadRequest.Headers.ContentDisposition;
                    uploadDirectoryRequest.UploadRequest.Headers.ContentEncoding = storageObject?.ContentEncoding ?? uploadDirectoryRequest.UploadRequest.Headers.ContentEncoding;
                };

                using TransferUtility transferUtility = new(CreateClient);
                await transferUtility.UploadDirectoryAsync(request);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError("Could not store directory to Spaces using S3");

                throw new StorageException("Could not store directory", e);
            }
        }

        /// <summary>
        ///     Remove directory and its contents.
        /// </summary>
        /// <param name="directoryPath">Full path of the directory to delete.</param>
        /// <returns>See <see cref="ValueTask"/>.</returns>
        public async Task RemoveDirectoryAsync(string directoryPath)
        {
            try
            {
                // NOTE: This call returns max. up to 1000 records by design.
                //       In order to obtain every record that matches our query, 
                //       we have to repeat our request execution in a loop - using continuation tokens -- until no longer truncated.
                //       See https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/S3/MS3ListObjectsV2AsyncListObjectsV2RequestCancellationToken.html
                ListObjectsV2Request request = new()
                {
                    BucketName = _options.BlobStorageName,
                    Prefix = directoryPath,
                    MaxKeys = MaxKeys
                };

                List<Task> tasklist = new();

                for (ListObjectsV2Response response = await CreateClient.ListObjectsV2Async(request);
                    response.IsTruncated;
                    request.ContinuationToken = response.NextContinuationToken, response = await CreateClient.ListObjectsV2Async(request))
                {
                    // TODO; Move this into for loop
                    if (response.S3Objects.Count <= 0)
                    {
                        break;
                    }

                    Task deleteTask = CreateClient.DeleteObjectsAsync(new()
                    {
                        BucketName = _options.BlobStorageName,
                        Objects = response.S3Objects.Select(x => new KeyVersion()
                        {
                            Key = x.Key
                        }).ToList()
                    });

                    tasklist.Add(deleteTask);

                    _ = Task.Run(() => deleteTask);
                }

                await Task.WhenAll(tasklist.ToArray());
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError("Could not delete directory on Spaces using S3");

                throw new StorageException("Could delete directory", e);
            }
        }

        /// <summary>
        ///     Test the Amazon S3 service backend.
        /// </summary>
        public async Task HealthCheck()
            => await CreateClient.ListBucketsAsync();
    }
}
