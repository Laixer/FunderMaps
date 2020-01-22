using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunderMaps.Tdm
{
    /// <summary>
    /// Abstraction to provide storage accounts from the connection names. 
    /// This gets the storage account name via the binding attribute's <see cref="IConnectionProvider.Connection"/>
    /// property. 
    /// If the connection is not specified on the attribute, it uses a default account. 
    /// </summary>
    public class StorageAccountProvider
    {
        public CloudStorageAccount StorageAccount { get; protected set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="configuration">Instance of <see cref="IConfiguration"/>.</param>
        public StorageAccountProvider(IConfiguration configuration)
            => StorageAccount = CloudStorageAccount.Parse(configuration.GetWebJobsConnectionString(ConnectionStringNames.Storage));

        /// <summary>
        /// Create blob client.
        /// </summary>
        /// <returns>See <see cref="CloudBlobClient"/>.</returns>
        public virtual CloudBlobClient CreateCloudBlobClient() => StorageAccount.CreateCloudBlobClient();

        /// <summary>
        /// Create queue client.
        /// </summary>
        /// <returns>See <see cref="CloudQueueClient"/>.</returns>
        public virtual CloudQueueClient CreateCloudQueueClient() => StorageAccount.CreateCloudQueueClient();

        /// <summary>
        /// Create table client.
        /// </summary>
        /// <returns>See <see cref="CloudTableClient"/>.</returns>
        public virtual CloudTableClient CreateCloudTableClient() => StorageAccount.CreateCloudTableClient();

        /// <summary>
        /// Create file client.
        /// </summary>
        /// <returns>See <see cref="CloudFileClient"/>.</returns>
        public virtual CloudFileClient CreateCloudFileClient() => StorageAccount.CreateCloudFileClient();
    }
}
