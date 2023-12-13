using System.CommandLine;
using FunderMaps.Core.ExternalServices.FunderMaps;
using FunderMaps.WsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var authkeyOption = new Option<string>(["-k", "--key"], "Webservice authkey")
{
    IsRequired = true
};

var baseUrlOption = new Option<string>(["-u", "--url"], "Webservice base url");

var logLevelOption = new Option<LogLevel>(["--log", "--log-level"], getDefaultValue: () => LogLevel.Information, "The log level to use");

var buildingArgument = new Argument<string>("building", "Building identifier");

var command = new RootCommand("FunderMaps command line interface")
{
    authkeyOption,
    baseUrlOption,
    logLevelOption,
    buildingArgument,
};

command.SetHandler(async (authkey, buildingId, baseUrl, logLevel) =>
{
    await using var serviceProvider = new ServiceCollection()
        .AddLogging(options =>
        {
            options.ClearProviders();
            options.AddSimpleConsole();
            options.SetMinimumLevel(logLevel);
        })
        .AddScoped<FunderMapsClient>()
        .Configure<FunderMapsOptions>(options =>
        {
            options.BaseUrl = baseUrl;
            options.ApiKey = authkey;
        })
        .AddScoped<WebserviceClientLogger>()
        .BuildServiceProvider();

    var funderMapsWebserviceClient = serviceProvider.GetRequiredService<WebserviceClientLogger>();

    await funderMapsWebserviceClient.LogAnalysisAsync(buildingId);
    // await funderMapsWebserviceClient.LogStatisticsAsync(buildingId);
}, authkeyOption, buildingArgument, baseUrlOption, logLevelOption);

await command.InvokeAsync(args);
