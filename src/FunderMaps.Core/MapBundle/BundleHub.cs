using FunderMaps.Core.Abstractions;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.MapBundle.Jobs;
using FunderMaps.Core.Threading;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Process bundles.
    /// </summary>
    internal class BundleHub : AppServiceBase, IBundleService
    {
        /// <summary>
        ///     Random interval determines if all bundles will be processed.    
        /// </summary>
        /// <remarks>
        ///     The random selection is based on the batch interval in minutes.
        ///     Increasing this value will make it less likely to process all bundles.
        /// </remarks>
        private readonly int randomInterval;

        private readonly ILogger _logger;
        private readonly IBundleRepository _bundleRepository;
        private readonly BackgroundTaskDispatcher _backgroundTaskDispatcher;
        private readonly Random _random = new();

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleHub(
            AppContext appContext,
            ILogger<BundleHub> logger,
            IBundleRepository bundleRepository,
            BackgroundTaskDispatcher backgroundTaskDispatcher,
            IConfiguration configuration)
        {
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
            _backgroundTaskDispatcher = backgroundTaskDispatcher ?? throw new ArgumentNullException(nameof(backgroundTaskDispatcher));

            randomInterval = configuration.GetValue<int>("Bundle:RandomInterval", 10);
        }

        /// <summary>
        ///     Select the bundles which need to be rebuild.
        /// </summary>
        /// <remarks>
        ///     Try to process all the bundles once every so many times.
        /// </remarks>
        private IAsyncEnumerable<Bundle> GetBuildCandidates()
            => _random.Next(0, randomInterval) == 0
                ? _bundleRepository.ListAllAsync(Navigation.All)
                : _bundleRepository.ListAllRecentAsync(Navigation.All);

        /// <summary>
        ///     Send build candidates off to background worker.
        /// </summary>
        public async Task BuildAsync()
        {
            await foreach (Bundle bundle in GetBuildCandidates())
            {
                _logger.LogDebug($"Enqueue bundle {bundle.Id}");

                await _backgroundTaskDispatcher.EnqueueTaskAsync<BundleJob>(new BundleBuildingContext
                {
                    Bundle = bundle,
                    Formats = new List<GeometryFormat> { GeometryFormat.MapboxVectorTiles },
                });
            }
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
