using FunderMaps.Core.Threading.Command;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Model.Jobs;

/// <summary>
///     Refresh job entry.
/// </summary>
public class RefreshJob : CommandTask
{
    private const string TaskName = "MODEL_REFRESH";

    private static readonly string[] concurrent_models = new string[]
    {
        "data.analysis_complete",
        "data.statistics_product_buildings_restored",
        "data.statistics_product_data_collected",
    };

    private static readonly string[] sequential_models = new string[]
    {
        "data.statistics_product_construction_years",
        "data.statistics_product_foundation_risk",
        "data.statistics_product_foundation_type",
        "data.statistics_product_incidents",
        "data.statistics_product_inquiries",
    };

    private readonly string connectionString;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public RefreshJob(IConfiguration configuration, ILogger<RefreshJob> logger)
    {
        // TODO: HACK
        Logger = logger;
        connectionString = configuration.GetConnectionString("FunderMapsAdminConnection");
    }

    /// <summary>
    ///     Run the background command.
    /// </summary>
    /// <param name="context">Command task execution context.</param>
    public override async Task ExecuteCommandAsync(CommandTaskContext context)
    {
        // TODO: HACK
        Context = context;

        // TODO: HACK.
        CreateDirectory("out");

        foreach (var model in concurrent_models)
        {
            CommandInfo command = new("psql");

            new PostgreSQLSink(connectionString).ParseCommandString(command);

            command.ArgumentList.Add("-c");
            command.ArgumentList.Add($"REFRESH MATERIALIZED VIEW CONCURRENTLY {model} WITH DATA");

            await RunCommandAsync(command);
        }

        foreach (var model in sequential_models)
        {
            CommandInfo command = new("psql");

            new PostgreSQLSink(connectionString).ParseCommandString(command);

            command.ArgumentList.Add("-c");
            command.ArgumentList.Add($"REFRESH MATERIALIZED VIEW {model} WITH DATA");

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
