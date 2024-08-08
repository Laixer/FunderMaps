using FunderMaps.Core;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Worker.Tasks;

internal sealed class ModelExportTask(
    IBlobStorageService blobStorageService,
    IAnalysisRepository analysisRepository,
    ILogger<ModelExportTask> logger) : ITaskService
{
    // TODO: Move into service/helper. See #801
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

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            string filePath = $"model_export.csv";

            await WriteCsvAsync(filePath, analysisRepository.ListAllAsync(Navigation.All), cancellationToken);

            DateTime currentDate = DateTime.Now;
            string dateString = currentDate.ToString("yyyy-MM-dd");

            logger.LogInformation("Uploading model export");

            await blobStorageService.StoreFileAsync($"model/{dateString}.csv", filePath);
        }
        finally
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "csv");
        }
    }
}
