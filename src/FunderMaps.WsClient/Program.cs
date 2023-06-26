using System.CommandLine;
using FunderMaps.AspNetCore.Services;
using FunderMaps.WsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

class Program
{
    internal static ServiceProvider SetupServiceProvider(Authentication authentication)
    {
        return new ServiceCollection()
            .AddLogging(options =>
            {
                options.ClearProviders();
                options.AddSimpleConsole();
                options.SetMinimumLevel(LogLevel.Debug);
            })
            .AddScoped<WebserviceClient>(serviceProvider => new(authentication))
            .AddScoped<WebserviceClientLogger>()
            .BuildServiceProvider();
    }

    internal static async Task DoCall(ServiceProvider serviceProvider, string buildingId)
    {
        var client = serviceProvider.GetRequiredService<WebserviceClientLogger>();

        await client.LogAnalysisAsync(buildingId);
        // await client.LogStatisticsAsync(buildingId);
    }

    internal static void TearDownServiceProvider(ServiceProvider serviceProvider)
    {
        serviceProvider.Dispose();
    }

    public static async Task<int> Main(string[] args)
    {
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
            var auth = new Authentication()
            {
                Email = username ?? throw new ArgumentNullException(nameof(username)),
                Password = password ?? throw new ArgumentNullException(nameof(password)),
            };

            var serviceProvider = SetupServiceProvider(auth);

            await DoCall(serviceProvider, buildingId);

            TearDownServiceProvider(serviceProvider);

        }, usernameOption, passwordOption, buildingArgument);

        return await command.InvokeAsync(args);
    }
}
