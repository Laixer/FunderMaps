using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using FunderMaps.Interfaces;
using System.IO;

namespace FunderMaps.Services
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

        protected CloudBlockBlob PrepareBlob(string filename, BlobProperties properties)
        {
            CloudBlockBlob cloudBlockBlob = SetContainer("report").GetBlockBlobReference(filename);

            cloudBlockBlob.Properties.CacheControl = properties.CacheControl;
            cloudBlockBlob.Properties.ContentMD5 = properties.ContentMD5;
            cloudBlockBlob.Properties.ContentEncoding = properties.ContentEncoding;
            cloudBlockBlob.Properties.ContentLanguage = properties.ContentLanguage;
            cloudBlockBlob.Properties.ContentDisposition = properties.ContentDisposition;

            return cloudBlockBlob;
        }

        protected CloudBlockBlob PrepareBlob(string filename)
        {
            return PrepareBlob(filename, new BlobProperties());
        }

        public async Task StoreFileAsync(string name, byte[] content)
        {
            await PrepareBlob(name).UploadFromByteArrayAsync(content, 0, 0);
        }

        public async Task StoreFileAsync(string name, Stream stream)
        {
            await PrepareBlob(name).UploadFromStreamAsync(stream);
        }
    }
}
