using FunderMaps.Console.Command;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunderMaps.Console.BackgroundTasks
{
    internal class DummyCommand : CommandTask
    {
        private ILogger<DummyCommand> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DummyCommand(ILogger<DummyCommand> logger)
        {
            _logger = logger;
        }

        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            _logger.LogDebug("START");

            await RunCommand("printenv");

            _logger.LogDebug("ENDS");
        }

        public override bool CanHandle(object value) => true;
    }
}
