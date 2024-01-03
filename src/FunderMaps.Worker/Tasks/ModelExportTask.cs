using FunderMaps.Core;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Worker.Tasks;

/// <summary>
///     Construct new instance.
/// </summary>
internal sealed class ModelExportTask(
    IBlobStorageService blobStorageService,
    IAnalysisRepository analysisRepository,
    ILogger<ModelExportTask> logger) : ITaskService
{
    // TODO: Move into service/helper.
    /// <summary>
    ///    Write CSV file.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <param name="productCalls">Product calls.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    private static async Task WriteCsvAsync(string filePath, IAsyncEnumerable<AnalysisProduct> productCalls, CancellationToken cancellationToken = default)
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
        try
        {
            string filePath = $"model_export.csv";

            await WriteCsvAsync(filePath, analysisRepository.ListAllAsync(Navigation.All), cancellationToken);

            DateTime currentDate = DateTime.Now;
            string dateString = currentDate.ToString("yyyy-MM-dd");

            logger.LogInformation("Uploading model export");

            await blobStorageService.StoreFileAsync($"model/export_{dateString}.csv", filePath);
        }
        finally
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "csv");
        }
    }
}
