using FunderMaps.Console.Types;
using FunderMaps.Core.Managers;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Console.Dev
{
    // TODO Remove
    /// <summary>
    ///     Dev class that periodically enqueues a task for us.
    /// </summary>
    public class DevEnqueueMachine : BackgroundService
    {
        private readonly QueueManager _queueManager;
        private readonly Random random = new Random();

        public DevEnqueueMachine(QueueManager queueManager)
            => _queueManager = queueManager ?? throw new ArgumentNullException(nameof(queueManager));

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _queueManager.EnqueueTask(new BundleBuildingContext
            {
                BundleId = new Guid("06df6716-2af0-4359-b2e5-c8a207c99b82"),
                Formats = new GeometryExportFormat[] { GeometryExportFormat.Gpkg },
            });

            return Task.CompletedTask;
        }
    }
}
