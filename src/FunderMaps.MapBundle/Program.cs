using FunderMaps.MapBundle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddJsonFile("/etc/fundermaps/appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var connectionString = configuration.GetConnectionString("FunderMapsConnection");

await using var serviceProvider = new ServiceCollection()
    .AddLogging(options =>
    {
        options.ClearProviders();
        options.AddSimpleConsole();
        options.SetMinimumLevel(LogLevel.Debug);
    })
    .AddSingleton<IConfiguration>(configuration)
    .AddFunderMapsAspNetCoreServicesNew()
    .AddScoped<BundleProcessor>()
    .BuildServiceProvider();

await serviceProvider.GetRequiredService<BundleProcessor>().RunAsync();
