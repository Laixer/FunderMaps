using FunderMaps.Core.Abstractions;
using FunderMaps.Core.Model.Jobs;
using FunderMaps.Core.Threading;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Model
{
    /// <summary>
    ///     Process bundles.
    /// </summary>
    internal class ModelService : AppServiceBase, IModelService
    {
        private readonly BackgroundTaskDispatcher _backgroundTaskDispatcher;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ModelService(BackgroundTaskDispatcher backgroundTaskDispatcher)
            => _backgroundTaskDispatcher = backgroundTaskDispatcher ?? throw new ArgumentNullException(nameof(backgroundTaskDispatcher));

        /// <summary>
        ///     Dispatch refresh job to background worker.
        /// </summary>
        public async Task UpdateAllModelsAsync()
        {
            await _backgroundTaskDispatcher.EnqueueTaskAsync<RefreshJob>();
        }
    }
}
