using FunderMaps.Tdm.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Tdm;
{
    public sealed class DownloadMedia
    {
        private const string sharedStateName = "syncstate";

        private readonly TdmService _tdmService;
        private readonly FunctionSharedState _functionState;
        private readonly StorageAccountProvider _storageAccount;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public DownloadMedia(TdmService tdmService, FunctionSharedState sharedState, StorageAccountProvider storageAccount)
        {
            _tdmService = tdmService ?? throw new ArgumentNullException(nameof(tdmService));
            _functionState = sharedState ?? throw new ArgumentNullException(nameof(sharedState));
            _storageAccount = storageAccount ?? throw new ArgumentNullException(nameof(storageAccount));
        }

        [FunctionName("DownloadMedia")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log, CancellationToken token)
        {
            // NOTE: Unfortunately function state creation has to happen when the function is called
            //       since we're not allowed to block the ctor. It's a waste of time to check this every single
            //       time the function is called.
            if (!await _functionState.ExistsAsync(sharedStateName).ConfigureAwait(false))
            {
                log.LogWarning("Shared function state does not exist");

                await _functionState.UpdateAsync(sharedStateName, new SharedSyncState()).ConfigureAwait(false);
            }

            var syncState = await _functionState.GetAsync<SharedSyncState>(sharedStateName).ConfigureAwait(false);

            var objectList = await _tdmService.SyncService.WonenSynchronizeAsync((int)syncState.SyncpointWonen).ConfigureAwait(false);

            foreach (var item in objectList.SynchronizationResponse.SynchronizationCollection.SynchronizationItem)
            {
                //
            }

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
