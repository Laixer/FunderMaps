using System.CommandLine;
using FunderMaps.Core.ExternalServices.FunderMaps;
using FunderMaps.WsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var usernameOption = new Option<string>(new[] { "-u", "--username" }, "Webservice username");
var passwordOption = new Option<string>(new[] { "-p", "--password" }, "Webservice password");

var authkeyOption = new Option<string>(new[] { "-k", "--key" }, "Webservice authkey")
{
    IsRequired = true
};

var logLevel = new Option<LogLevel>(new[] { "--log", "--log-level" }, getDefaultValue: () => LogLevel.Information, "The log level to use");

var buildingArgument = new Argument<string>("building", "Building identifier");

var command = new RootCommand("FunderMaps command line interface")
{
    usernameOption,
    passwordOption,
    authkeyOption,
    logLevel,
    buildingArgument,
};

command.SetHandler(async (authkey, username, password, buildingId, logLevel) =>
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
            options.BaseUrl = "https://ws-staging.fundermaps.com";
            // options.BaseUrl = "http://localhost:5000";
            options.Email = username;
            options.Password = password;
            options.ApiKey = authkey;
        })
        .AddScoped<WebserviceClientLogger>()
        .BuildServiceProvider();

    var funderMapsWebserviceClient = serviceProvider.GetRequiredService<WebserviceClientLogger>();

    await funderMapsWebserviceClient.LogAnalysisAsync(buildingId);
    // await funderMapsWebserviceClient.LogStatisticsAsync(buildingId);
}, authkeyOption, usernameOption, passwordOption, buildingArgument, logLevel);

await command.InvokeAsync(args);
