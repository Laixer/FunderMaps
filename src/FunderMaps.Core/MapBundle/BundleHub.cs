using FunderMaps.Core.Abstractions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.MapBundle.Jobs;
using FunderMaps.Core.Threading;
using FunderMaps.Core.Types;
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
        private readonly ILogger _logger;
        private readonly IBundleRepository _bundleRepository;
        private readonly BackgroundTaskDispatcher _backgroundTaskDispatcher;
        private readonly Random _random = new Random();

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleHub(AppContext appContext, ILogger<BundleHub> logger, IBundleRepository bundleRepository, BackgroundTaskDispatcher backgroundTaskDispatcher)
        {
            AppContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
            _backgroundTaskDispatcher = backgroundTaskDispatcher ?? throw new ArgumentNullException(nameof(backgroundTaskDispatcher));
        }

        /// <summary>
        ///     Build bundles.
        /// </summary>
        /// <remarks>
        ///     Try to process all the bundles once every so many times.
        /// </remarks>>
        public async Task BuildAsync()
        {
            await foreach (var bundle in _random.Next(0, 10) == 0
                ? _bundleRepository.ListAllAsync(Navigation.All)
                : _bundleRepository.ListAllRecentAsync(Navigation.All))
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
