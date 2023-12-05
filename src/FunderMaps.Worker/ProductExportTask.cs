using FunderMaps.Core.Entities;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Worker;

public class ProductExportTask : ISingleShotTask
{
    private readonly ITelemetryRepository _telemetryRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public ProductExportTask(
        IBlobStorageService blobStorageService,
        ITelemetryRepository telemetryRepository,
        ILogger<ProductExportTask> logger)
    {
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        _telemetryRepository = telemetryRepository ?? throw new ArgumentNullException(nameof(telemetryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await foreach (var organization in _telemetryRepository.ListLastMonthOrganizationaAsync())
        {
            try
            {
                string filePath = $"product_tracker_{organization}.csv";

                await WriteCsvAsync(filePath, _telemetryRepository.ListLastMonthByOrganizationIdAsync(organization), cancellationToken);

                var currentDate = DateTime.Now;
                var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

                _logger.LogInformation("Uploading product tracker file for {Organization} for {Month}", organization, lastDayOfLastMonth);

                string lastMonthName = lastDayOfLastMonth.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);
                await _blobStorageService.StoreFileAsync($"product/export_{lastMonthName.ToLower()}_{organization}.csv", filePath);
            }
            finally
            {
                var currentDirectory = Directory.GetCurrentDirectory();

                FileHelper.DeleteFilesWithExtension(currentDirectory, "csv");
            }
        }
    }
}