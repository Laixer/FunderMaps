using System.CommandLine;
using FunderMaps.AspNetCore.Services;
using FunderMaps.WsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var usernameOption = new Option<string?>("--username", "Webservice username")
{
    IsRequired = true,
};
usernameOption.AddAlias("-u");

var passwordOption = new Option<string?>("--password", "Webservice password")
{
    IsRequired = true,
};
passwordOption.AddAlias("-p");

var buildingArgument = new Argument<string>("building", "Building identifier")
{
    Arity = ArgumentArity.ExactlyOne,
};

var command = new RootCommand("FunderMaps command line interface")
{
    usernameOption,
    passwordOption,
    buildingArgument,
};

command.SetHandler(async (username, password, buildingId) =>
{
    await using var serviceProvider = new ServiceCollection()
        .AddLogging(options =>
        {
            options.ClearProviders();
            options.AddSimpleConsole();
            options.SetMinimumLevel(LogLevel.Debug);
        })
        .AddScoped<FunderMapsClient>()
        .Configure<FunderMapsClientOptions>(options =>
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

}, usernameOption, passwordOption, buildingArgument);

await command.InvokeAsync(args);
