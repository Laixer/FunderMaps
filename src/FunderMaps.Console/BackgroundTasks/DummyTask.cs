using FunderMaps.Core.Threading;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunderMaps.Console.BundleServices
{
    internal class DummyTask : BackgroundTask
    {
        private ILogger<DummyTask> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DummyTask(ILogger<DummyTask> logger)
        {
            _logger = logger;
        }

        public override async Task ExecuteAsync(BackgroundTaskContext context)
        {
            _logger.LogDebug("START");
            
            await Task.Delay(2000);
            
            _logger.LogDebug("ENDS");
        }

        public override bool CanHandle(object value) => true;
    }
}
