using FunderMaps.Core.Entities;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Worker.Tasks;

internal sealed class ProductExportTask(
    IBlobStorageService blobStorageService,
    ITelemetryRepository telemetryRepository,
    ILogger<ProductExportTask> logger) : ITaskService
{
    // TODO: Move into service/helper. See #801
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

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await foreach (var organization in telemetryRepository.ListLastMonthOrganizationaAsync())
        {
            try
            {
                string filePath = $"product_tracker_{organization}.csv";

                await WriteCsvAsync(filePath, telemetryRepository.ListLastMonthByOrganizationIdAsync(organization), cancellationToken);

                var currentDate = DateTime.Now;
                var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

                logger.LogInformation("Uploading product tracker file for {Organization} for {Month}", organization, lastDayOfLastMonth);

                string yearName = lastDayOfLastMonth.ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string lastMonthName = lastDayOfLastMonth.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);
                await blobStorageService.StoreFileAsync($"product/{yearName}_{lastMonthName.ToLower()}_{organization}.csv", filePath);
            }
            finally
            {
                var currentDirectory = Directory.GetCurrentDirectory();

                FileHelper.DeleteFilesWithExtension(currentDirectory, "csv");
            }
        }
    }
}
