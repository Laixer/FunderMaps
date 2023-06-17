using System.Text.Json;
using FunderMaps.AspNetCore.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddScoped<WebserviceClient>(_ => new(new()
    {
        Email = "",
        Password = "",
    }, "http://localhost:5000/")
    )
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
