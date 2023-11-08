using FunderMaps.Core.Email;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunderMaps.ProductExporter;

public class ProductExporter : SingleShotService
{
    private readonly IBlobStorageService _blobStorageService;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public ProductExporter(
        HealthCheckService healthCheckService,
        IServiceScopeFactory serviceScopeFactory,
        IBlobStorageService blobStorageService,
        IEmailService emailService,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<ProductExporter> logger)
        : base(healthCheckService, serviceScopeFactory, emailService, hostApplicationLifetime, logger)
    {
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
    }

    /// <summary>
    ///    Write CSV file.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <param name="productCalls">Product calls.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    private static async Task WriteCsvAsync(string filePath, IAsyncEnumerable<ProductCall> productCalls, CancellationToken cancellationToken = default)
    {
        using var writer = new StreamWriter(filePath);

        var csvConfig = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using var csv = new CsvHelper.CsvWriter(writer, csvConfig);
        await csv.WriteRecordsAsync(productCalls, cancellationToken);
    }

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    protected override async Task RunAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        var telemetryRepository = scope.ServiceProvider.GetRequiredService<ITelemetryRepository>();

        await foreach (var organization in telemetryRepository.ListLastMonthOrganizationaAsync())
        {
            try
            {
                string filePath = $"product_tracker_{organization}.csv";

                await WriteCsvAsync(filePath, telemetryRepository.ListLastMonthByOrganizationIdAsync(organization), cancellationToken);

                var currentDate = DateTime.Now;
                var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

                string lastMonthName = lastDayOfLastMonth.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);

                await _blobStorageService.StoreFileAsync($"product/export_{lastMonthName.ToLower()}_{organization}.csv", filePath);
            }
            finally
            {
                var currentDirectory = Directory.GetCurrentDirectory();

                FileHelper.DeleteFilesWithExtension(currentDirectory, "csv");
            }
        }

        await _emailService.SendAsync(new EmailMessage
        {
            Subject = "FunderMaps product",
            Content = "FunderMaps product export complete for last month.",
        }, cancellationToken);
    }
}
