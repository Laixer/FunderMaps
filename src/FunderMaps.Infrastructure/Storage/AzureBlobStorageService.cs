using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Infrastructure.Storage
{
    /// <summary>
    ///     Azure Blob Storage service.
    /// </summary>
    internal class AzureBlobStorageService : IBlobStorageService
    {
        private readonly BlobStorageOptions _options;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly StorageSharedKeyCredential _sharedKeyCredential;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="options">File service options.</param>
        /// <param name="config">Application configuration.</param>
        public AzureBlobStorageService(IOptions<BlobStorageOptions> options, IConfiguration config)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _sharedKeyCredential = StorageSharedKeyCredentialFromConnectionString(config.GetConnectionString("AzureStorageConnectionString"));
            _blobServiceClient = new BlobServiceClient(config.GetConnectionString("AzureStorageConnectionString"));
        }

        /// <summary>
        ///     Use the connection string to generate a <see cref="StorageSharedKeyCredential"/>.
        /// </summary>
        /// <remarks>
        ///     This should be the task of the SDK.
        /// </remarks>
        /// <param name="connectionString">The Azure Storage connection string.</param>
        /// <returns><see cref="StorageSharedKeyCredential"/>.</returns>
        public static StorageSharedKeyCredential StorageSharedKeyCredentialFromConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
            return new StorageSharedKeyCredential((string)builder["AccountName"], (string)builder["AccountKey"]);
        }

        /// <summary>
        ///     Prepare blob for storage.
        /// </summary>
        /// <param name="containerName">Where to store the file.</param>
        /// <param name="blobName">The actual file on disk.</param>
        /// <returns>The blob block.</returns>
        protected Task<BlobClient> PrepareBlobAsync(string containerName, string blobName)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_options.StorageContainers.TryGetValue(containerName, out string store) ? store : containerName);
            BlobClient blobClient = container.GetBlobClient(blobName);
            return Task.FromResult(blobClient);
        }

        /// <summary>
        ///     Check if a file exist in storage.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>True if file exist, false otherwise.</returns>
        public async ValueTask<bool> FileExistsAsync(string containerName, string fileName)
        {
            BlobClient blobClient = await PrepareBlobAsync(containerName, fileName);
            return blobClient.Exists();
        }

        /// <summary>
        ///     Retrieve file access link.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="hoursValid">How long the link is valid in hours.</param>
        /// <returns>The generated link.</returns>
        public async ValueTask<Uri> GetAccessLinkAsync(string containerName, string fileName, double hoursValid = 1)
        {
            BlobClient blobClient = await PrepareBlobAsync(containerName, fileName);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(hoursValid),
            };
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

            return new UriBuilder(blobClient.Uri)
            {
                Query = sasBuilder.ToSasQueryParameters(_sharedKeyCredential).ToString()
            }.Uri;
        }

        /// <summary>
        ///     Store the file in the blob storage account.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="stream">Content stream.</param>
        public async ValueTask StoreFileAsync(string containerName, string fileName, Stream stream)
        {
            BlobClient blobClient = await PrepareBlobAsync(containerName, fileName);
            await blobClient.UploadAsync(stream);
        }

        /// <summary>
        ///     Store the file in the blob storage account.
        /// </summary>
        /// <param name="containerName">Storage container.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="contentType">Blob content type.</param>
        /// <param name="stream">Content stream.</param>
        public async ValueTask StoreFileAsync(string containerName, string fileName, string contentType, Stream stream)
        {
            BlobClient blobClient = await PrepareBlobAsync(containerName, fileName);

            var uploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                },
            };

            await blobClient.UploadAsync(stream, uploadOptions);
        }
    }
}
