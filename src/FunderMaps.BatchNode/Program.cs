using System.CommandLine;

namespace FunderMaps.BatchNode;

/// <summary>
///     Application entry.
/// </summary>
public static class Program
{
    /// <summary>
    ///     Configuration.
    /// </summary>
    public static IConfiguration Configuration { get; set; }

    /// <summary>
    ///     Host environment.
    /// </summary>
    public static IHostEnvironment HostEnvironment { get; set; }

    /// <summary>
    ///     Application entry point.
    /// </summary>
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var serviceProvider = host.Services;

        RootCommand rootCommand = new();

        Command verbCommand = new("map-bundle");
        verbCommand.SetHandler(async () =>
        {
            Console.WriteLine($"Running map-bundle job");

            var exportJob = serviceProvider.GetRequiredService<Core.MapBundle.Jobs.ExportJob>();
            await exportJob.ExecuteCommandAsync(new Core.Threading.Command.CommandTaskContext(Guid.NewGuid())
            {
                Workspace = System.Environment.CurrentDirectory + "/workspace",
                KeepWorkspace = true,
                Failed = false,
            });
        });
        rootCommand.Add(verbCommand);

        // TODO: Should be updated. This job should export the entire analysis_complete.
        Command exportCommand = new("export");
        exportCommand.SetHandler(async () =>
        {
            Console.WriteLine($"Running export job");

            var exportJob = serviceProvider.GetRequiredService<Core.MapBundle.Jobs.ExportGpkg>();
            await exportJob.ExecuteCommandAsync(new Core.Threading.Command.CommandTaskContext(Guid.NewGuid())
            {
                Workspace = System.Environment.CurrentDirectory + "/workspace",
                KeepWorkspace = true,
                Failed = false,
            });
        });
        rootCommand.Add(exportCommand);

        Command refreshCommand = new("refresh");
        refreshCommand.SetHandler(async () =>
        {
            Console.WriteLine($"Running refresh job");

            var scope = serviceProvider.CreateAsyncScope();

            var exportJob = scope.ServiceProvider.GetRequiredService<Core.Model.Jobs.RefreshJob>();
            await exportJob.ExecuteCommandAsync(new Core.Threading.Command.CommandTaskContext(Guid.NewGuid())
            {
                Workspace = System.Environment.CurrentDirectory + "/workspace",
                KeepWorkspace = true,
                Failed = false,
            });
        });
        rootCommand.Add(refreshCommand);

        rootCommand.Description = "FunderMaps batch job runner";

        await rootCommand.InvokeAsync(args);
    }

    /// <summary>
    ///     Build a host and run the application.
    /// </summary>
    /// <remarks>
    ///     The signature of this method should not be changed.
    ///     External tooling expects this function be present.
    /// </remarks>
    /// <param name="args">Commandline arguments.</param>
    /// <returns>See <see cref="IHostBuilder"/>.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                // Configure FunderMaps services.
                services.AddFunderMapsCoreServices();
                services.AddFunderMapsInfrastructureServices();
                services.AddFunderMapsDataServices("FunderMapsConnection");

                // The startup essential properties can be used to setup components.
                (Configuration, HostEnvironment) = services.BuildStartupProperties();

                // Add the task scheduler.
                services.Configure<MapBundleOptions>(Configuration.GetSection(MapBundleOptions.Section));
                services.AddHostedService<TimedMapBundleService>();
            });
}
