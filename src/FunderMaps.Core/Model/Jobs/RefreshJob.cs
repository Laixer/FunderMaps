using System.Threading.Tasks;
using FunderMaps.Core.Threading.Command;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FunderMaps.Core.Model.Jobs
{
    /// <summary>
    ///     Refresh job entry.
    /// </summary>
    internal class RefreshJob : CommandTask
    {
        private const string TaskName = "MODEL_REFRESH";

        private static readonly string[] models = new string[]
        {
            "data.analysis_foundation_risk",
            "data.analysis_foundation_indicative",
            "data.analysis_complete",
        };

        private string connectionString;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RefreshJob(IConfiguration configuration, IMapService mapService)
        {
            connectionString = configuration.GetConnectionString("FunderMapsConnection");
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            foreach (var model in models)
            {
                CommandInfo command = new("psql");

                new PostgreSQLSink(connectionString).ParseCommandString(command);

                command.ArgumentList.Add("-c");
                command.ArgumentList.Add($"refresh materialized view {model} with data");

                await RunCommandAsync(command);
            }
        }

        /// <summary>
        ///     Method to check if a task can be handeld by this job.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name is not null && name.ToUpperInvariant() == TaskName;
    }
}
