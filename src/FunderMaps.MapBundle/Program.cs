using FunderMaps.MapBundle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
    {
        configurationBuilder.AddJsonFile("/etc/fundermaps/appsettings.json", optional: true);
    })
    .ConfigureServices((hostBuilderContext, services) =>
    {
        services.AddFunderMapsAspNetCoreServices();
        services.AddHostedService<HostedBundleProcessor>();
    })
    .UseSystemd()
    .Build()
    .RunAsync();
