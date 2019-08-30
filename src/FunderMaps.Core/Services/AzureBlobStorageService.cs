using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
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
        protected readonly CloudBlobClient _blobClient;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="config">Application configuration.</param>
        public AzureBlobStorageService(IConfiguration config)
        {
            _storageAccount = CloudStorageAccount.Parse(config.GetConnectionString("AzureStorageConnectionString"));
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        protected CloudBlobContainer SetContainer(string name) => _blobClient.GetContainerReference(name);

        protected CloudBlockBlob PrepareBlob(string container, string filename, BlobProperties properties)
        {
            CloudBlockBlob cloudBlockBlob = SetContainer(container).GetBlockBlobReference(filename);

            cloudBlockBlob.Properties.CacheControl = properties.CacheControl;
            cloudBlockBlob.Properties.ContentMD5 = properties.ContentMD5;
            cloudBlockBlob.Properties.ContentType = properties.ContentType;
            cloudBlockBlob.Properties.ContentEncoding = properties.ContentEncoding;
            cloudBlockBlob.Properties.ContentLanguage = properties.ContentLanguage;
            cloudBlockBlob.Properties.ContentDisposition = properties.ContentDisposition;

            return cloudBlockBlob;
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
