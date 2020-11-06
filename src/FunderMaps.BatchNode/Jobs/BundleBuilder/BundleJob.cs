using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FunderMaps.BatchNode.Command;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunderMaps.BatchNode.Jobs.BundleBuilder
{
    internal class BundleJob : CommandTask
    {
        private ILogger<BundleJob> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleJob(IServiceScopeFactory serviceScopeFactory,
            // IBundleRepository bundleRepository,
            // ILayerRepository layerRepository,
            ILogger<BundleJob> logger)
            : base(logger)
        {
            _logger = logger;
        }

        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            _logger.LogDebug("START");

            await Task.CompletedTask;

            _logger.LogDebug("ENDS");
        }

        /// <summary>
        ///     Method to check if a task can be handeld by this job.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name.ToLowerInvariant() == "bundle_building" && value is string;
    }
}
