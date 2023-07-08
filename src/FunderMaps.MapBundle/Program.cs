using FunderMaps.Core.Services;
using FunderMaps.Core.Storage;
using FunderMaps.Data.Providers;
using FunderMaps.MapBundle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddLogging(options =>
            {
                options.ClearProviders();
                options.AddSimpleConsole();
                options.SetMinimumLevel(LogLevel.Debug);
            })
            .AddFunderMapsCoreServices2()
            .AddFunderMapsDataServices()
            .Configure<DbProviderOptions>(options =>
            {
                options.ConnectionString = configuration["ConnectionStrings:FunderMapsConnection"];
                options.ApplicationName = "FunderMaps.MapBundle";
            })
            .Configure<MapboxOptions>(configuration.GetSection(MapboxOptions.Section))
            .Configure<BlobStorageOptions>(configuration.GetSection(BlobStorageOptions.Section))
            .AddScoped<BundleProcessor>()
            .BuildServiceProvider();

        await serviceProvider.GetRequiredService<BundleProcessor>().RunAsync();

        await serviceProvider.DisposeAsync();
    }
}