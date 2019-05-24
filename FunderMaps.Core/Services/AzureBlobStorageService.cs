using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using FunderMaps.Core.Helpers;

using FunderMaps.Core.Interfaces;

namespace FunderMaps.Core.Services
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly CloudStorageAccount _storageAccount;
        protected readonly CloudBlobClient _blobClient;

        public AzureBlobStorageService(IConfiguration config)
        {
            _storageAccount = CloudStorageAccount.Parse(config.GetConnectionString("AzureStorageConnectionString"));
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        protected CloudBlobContainer SetContainer(string name) =>
            _blobClient.GetContainerReference(name);

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

        public async Task StoreFileAsync(string store, string name, byte[] content)
        {
            await PrepareBlob(store, name, new BlobProperties()).UploadFromByteArrayAsync(content, 0, 0);
        }

        public async Task StoreFileAsync(string store, string name, Stream stream)
        {
            await PrepareBlob(store, name, new BlobProperties()).UploadFromStreamAsync(stream);
        }

        public async Task StoreFileAsync(string store, ApplicationFile file, Stream stream)
        {
            await PrepareBlob(store, file.FileName, new BlobProperties
            {
                ContentType = file.ContentType
            }).UploadFromStreamAsync(stream);
        }
    }
}
