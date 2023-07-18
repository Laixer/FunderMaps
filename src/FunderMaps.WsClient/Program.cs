using System.CommandLine;
using FunderMaps.Core.ExternalServices.FunderMaps;
using FunderMaps.WsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var usernameOption = new Option<string?>(new[] { "-u", "--username" }, "Webservice username")
{
    IsRequired = true,
};

var passwordOption = new Option<string?>(new[] { "-p", "--password" }, "Webservice password")
{
    IsRequired = true,
};

var logLevel = new Option<LogLevel>(new[] { "--log", "--log-level" }, getDefaultValue: () => LogLevel.Information, "The log level to use");

var buildingArgument = new Argument<string>("building", "Building identifier")
{
    Arity = ArgumentArity.ExactlyOne,
};

var command = new RootCommand("FunderMaps command line interface")
{
    usernameOption,
    passwordOption,
    logLevel,
    buildingArgument,
};

command.SetHandler(async (username, password, buildingId, logLevel) =>
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
            options.Email = username ?? throw new ArgumentNullException(nameof(username));
            options.Password = password ?? throw new ArgumentNullException(nameof(password));
        })
        .AddScoped<WebserviceClientLogger>()
        .BuildServiceProvider();

    var funderMapsWebserviceClient = serviceProvider.GetRequiredService<WebserviceClientLogger>();

    await funderMapsWebserviceClient.LogAnalysisAsync(buildingId);
    // await funderMapsWebserviceClient.LogStatisticsAsync(buildingId);

}, usernameOption, passwordOption, buildingArgument, logLevel);

await command.InvokeAsync(args);
