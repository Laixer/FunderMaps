using FunderMaps.BatchNode.Command;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunderMaps.BatchNode.Jobs
{
    internal class DummyJob : CommandTask
    {
        private ILogger<DummyJob> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DummyJob(ILogger<DummyJob> logger)
        {
            _logger = logger;
        }

        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            _logger.LogDebug("START");

            await RunCommand("printenv", context.Value as string);

            _logger.LogDebug("ENDS");
        }

        public override bool CanHandle(string name, object value)
            => name.ToLowerInvariant() == "dummy" && value is string;
    }
}
