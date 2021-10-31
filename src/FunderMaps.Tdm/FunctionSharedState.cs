using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Tdm;
{
    public class FunctionSharedState
    {
        private readonly CloudBlobClient _client;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="client">Instance of <see cref="CloudBlobClient"/>.</param>
        public FunctionSharedState(StorageAccountProvider storageAccount)
        {
            if (storageAccount == null)
            {
                throw new ArgumentNullException(nameof(storageAccount));
            }

            _client = storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Get handle tot the blob block.
        /// </summary>
        /// <param name="name">Blob name.</param>
        /// <returns><see cref="CloudBlockBlob"/>.</returns>
        private async Task<CloudBlockBlob> GetCloudBlockBlob(string name)
        {
            var container = _client.GetContainerReference(HostContainerNames.Shared);
            await container.CreateIfNotExistsAsync().ConfigureAwait(false);
            return container.GetBlockBlobReference(name);
        }

        /// <summary>
        /// Check if shared state exists.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True when shared state exists, false otherwise.</returns>
        public virtual async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
            => await (await GetCloudBlockBlob(name).ConfigureAwait(false)).ExistsAsync().ConfigureAwait(false);

        /// <summary>
        /// Store string content as shared state.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="str">String contents.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public virtual async Task UpdateAsync(string name, string str, CancellationToken cancellationToken = default)
            => await (await GetCloudBlockBlob(name).ConfigureAwait(false)).UploadTextAsync(str).ConfigureAwait(false);

        /// <summary>
        /// Get shared state contents.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Return shared state contents.</returns>
        public virtual async Task<string> GetAsync(string name, CancellationToken cancellationToken = default)
            => await (await GetCloudBlockBlob(name).ConfigureAwait(false)).DownloadTextAsync().ConfigureAwait(false);
    }
}
