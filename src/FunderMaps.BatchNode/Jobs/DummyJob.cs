using FunderMaps.BatchNode.Command;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunderMaps.BatchNode.Jobs
{
    internal class DummyJob : CommandTask
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DummyJob(ILogger<DummyJob> logger)
            : base(logger)
        {
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            _logger.LogDebug("START");

            await RunCommandAsync("printenv", context.Value as string);

            _logger.LogDebug("ENDS");
        }

        public override bool CanHandle(string name, object value)
            => name.ToLowerInvariant() == "dummy" && value is string;
    }
}
