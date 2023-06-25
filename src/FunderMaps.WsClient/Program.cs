using System.Text.Json;
using FunderMaps.AspNetCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .Build();

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<IConfiguration>(configuration)
    .AddScoped<WebserviceClient>(serviceProvider =>
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();

        var domain = config.GetValue<string>("FunderMaps:Domain");
        var email = config.GetValue<string>("FunderMaps:Email") ?? throw new ArgumentNullException("FunderMaps:Email");
        var password = config.GetValue<string>("FunderMaps:Password") ?? throw new ArgumentNullException("FunderMaps:Password");

        return new WebserviceClient(new() { Email = email, Password = password }, domain);
    })
    .BuildServiceProvider();


//configure console logging
// serviceProvider
// .GetService<ILoggerFactory>();
// .AddConsole(LogLevel.Debug);

var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
    .CreateLogger<Program>();

logger.LogInformation("Starting application");

var client = serviceProvider.GetRequiredService<WebserviceClient>();

var stopwatch = new System.Diagnostics.Stopwatch();
stopwatch.Start();

var product = await client.GetAnalysisAsync("NL.IMBAG.PAND.0606100000009175");
Console.WriteLine($"Address ID: {product?.ExternalAddressId}");

if (product is not null)
{
    var statistics = await client.GetStatisticsAsync(product.NeighborhoodId);

    var options = new JsonSerializerOptions { WriteIndented = true };
    string jsonString = JsonSerializer.Serialize(statistics, options);

    Console.WriteLine(jsonString);
}

stopwatch.Stop();

Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
