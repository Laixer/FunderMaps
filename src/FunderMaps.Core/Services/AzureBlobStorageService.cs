﻿using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// <summary>
    /// Azure storage implementing file storage service.
    /// </summary>
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _blobClient;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="config">Application configuration.</param>
        public AzureBlobStorageService(IConfiguration config)
        {
            _storageAccount = CloudStorageAccount.Parse(config.GetConnectionString("AzureStorageConnectionString"));
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Set container name.
        /// </summary>
        /// <param name="name">Name of container.</param>
        /// <returns>Blob container.</returns>
        protected CloudBlobContainer SetContainer(string name) => _blobClient.GetContainerReference(name);

        /// <summary>
        /// Prepare blob for storage.
        /// </summary>
        /// <param name="container">Where to store the file.</param>
        /// <param name="filename">The actual file on disk.</param>
        /// <param name="properties">Blob properties.</param>
        /// <returns>The blob block.</returns>
        protected CloudBlockBlob PrepareBlob(string container, string filename, BlobProperties properties)
        {
            var cloudBlockBlob = SetContainer(container).GetBlockBlobReference(filename);

            cloudBlockBlob.Properties.CacheControl = properties.CacheControl;
            cloudBlockBlob.Properties.ContentMD5 = properties.ContentMD5;
            cloudBlockBlob.Properties.ContentType = properties.ContentType;
            cloudBlockBlob.Properties.ContentEncoding = properties.ContentEncoding;
            cloudBlockBlob.Properties.ContentLanguage = properties.ContentLanguage;
            cloudBlockBlob.Properties.ContentDisposition = properties.ContentDisposition;

            return cloudBlockBlob;
        }

        /// <summary>
        /// Retrieve file access link.
        /// </summary>
        /// <param name="store">Storage container.</param>
        /// <param name="name">File name.</param>
        /// <param name="hoursValid">How long the link is valid in hours.</param>
        /// <returns>The generated link.</returns>
        public string GetAccessLink(string store, string name, double hoursValid = 1)
        {
            var cloudBlockBlob = SetContainer(store).GetBlockBlobReference(name);

            // Generate the shared access signature on the blob, setting the constraints directly on the signature.
            var sasBlobToken = cloudBlockBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(hoursValid),
                Permissions = SharedAccessBlobPermissions.Read,
            });

            return cloudBlockBlob.Uri + sasBlobToken;
        }

        /// <summary>
        /// Retrieve account name for the storage service.
        /// </summary>
        /// <returns>Account name.</returns>
        public Task<string> StorageAccountAsync()
            => _blobClient.GetAccountPropertiesAsync().ContinueWith(t => t.Result.AccountKind);

        /// <summary>
        /// Store the file in the data store.
        /// </summary>
        /// <param name="store">Storage container.</param>
        /// <param name="name">File name.</param>
        /// <param name="content">Content array.</param>
        public Task StoreFileAsync(string store, string name, byte[] content)
            => PrepareBlob(store, name, new BlobProperties()).UploadFromByteArrayAsync(content, 0, 0);

        /// <summary>
        /// Store the file in the data store.
        /// </summary>
        /// /// <param name="store">Storage container.</param>
        /// <param name="name">File name.</param>
        /// <param name="stream">Content stream.</param>
        public Task StoreFileAsync(string store, string name, Stream stream)
            => PrepareBlob(store, name, new BlobProperties()).UploadFromStreamAsync(stream);

        /// <summary>
        /// Store the file in the data store.
        /// </summary>
        /// /// <param name="store">Storage container.</param>
        /// <param name="file">Application file.</param>
        /// <param name="stream">Content stream.</param>
        public Task StoreFileAsync(string store, ApplicationFile file, Stream stream)
            => PrepareBlob(store, file.FileName, new BlobProperties
            {
                ContentType = file.ContentType
            }).UploadFromStreamAsync(stream);
    }
}
