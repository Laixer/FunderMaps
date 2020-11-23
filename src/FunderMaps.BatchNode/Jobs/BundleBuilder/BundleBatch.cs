using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FunderMaps.BatchNode.Command;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Threading;
using FunderMaps.Data;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.BatchNode.Jobs.BundleBuilder
{
    /// <summary>
    ///     Bundle batch job entry.
    /// </summary>
    internal class BundleBatch : CommandTask
    {
        private const string TaskName = "bundle_batch";

        protected readonly BackgroundTaskDispatcher _backgroundTaskDispatcher;
        protected readonly IBundleRepository _bundleRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleBatch(
            BackgroundTaskDispatcher backgroundTaskDispatcher,
            IBundleRepository bundleRepository,
            ILogger<BundleJob> logger)
            : base(logger)
        {
            _backgroundTaskDispatcher = backgroundTaskDispatcher ?? throw new ArgumentNullException(nameof(backgroundTaskDispatcher));
            _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (JsonSerializer.Deserialize<BundleBuildingContext>(context.Value as string) is not BundleBuildingContext bundleBuildingContext)
            {
                throw new ProtocolException("Invalid bundle building context");
            }

            if (bundleBuildingContext.Formats is null || !bundleBuildingContext.Formats.Any())
            {
                Logger.LogWarning("No formats listed for export");
                return;
            }

            await foreach (var bundle in _bundleRepository.ListAllAsync(Navigation.All))
            {
                bundleBuildingContext.BundleId = bundle.Id;

                Logger.LogDebug($"Enqueue bundle {bundle.Id}");

                await _backgroundTaskDispatcher.EnqueueTaskAsync<BundleJob>(JsonSerializer.Serialize(bundleBuildingContext));

                await Task.Delay(500);
            }
        }

        /// <summary>
        ///     Method to check if a task can be handeld by this job.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name is not null && name.ToLowerInvariant() == TaskName && value is string;
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
