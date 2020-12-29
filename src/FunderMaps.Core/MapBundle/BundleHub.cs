using FunderMaps.Core.Abstractions;
using FunderMaps.Core.Interfaces;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Process bundles.
    /// </summary>
    internal class BundleHub : AppServiceBase, IBundleService
    {
        private const string TaskBuildName = "BUNDLE_BUILDING";
        private const string TaskBuildAllName = "BUNDLE_BATCH";
        
        private readonly IBatchService _batchService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleHub(AppContext appContext, IBatchService batchService)
            => (AppContext, _batchService) = (appContext, batchService);

        /// <summary>
        ///     Build a bundle.
        /// </summary>
        /// <param name="context">Bundle building context.</param>
        public Task<Guid> BuildAsync(BundleBuildingContext context)
            => _batchService.EnqueueAsync(TaskBuildName, context, AppContext.CancellationToken);

        /// <summary>
        ///     Build all bundles.
        /// </summary>
        /// <param name="context">Bundle building context.</param>
        public Task<Guid> BuildAllAsync(BundleBuildingContext context)
            => _batchService.EnqueueAsync(TaskBuildAllName, context, AppContext.CancellationToken);
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
