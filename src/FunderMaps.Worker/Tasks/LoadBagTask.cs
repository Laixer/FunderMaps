using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Worker.Tasks;

/// <summary>
///     Construct new instance.
/// </summary>
internal sealed class LoadBagTask(
    IOptions<DbProviderOptions> dbProviderOptions,
    // IBlobStorageService blobStorageService,
    IGDALService gdalService,
    IOperationRepository operationRepository,
    ILogger<LoadBagTask> logger) : ITaskService
{
    private const string FileUrl = "https://service.pdok.nl/lv/bag/atom/downloads/bag-light.gpkg";
    private const int MinimumFileSize = 1024 * 1024; // 1 MB

    private readonly DbProviderOptions _dbProviderOptions = dbProviderOptions?.Value ?? throw new ArgumentNullException(nameof(dbProviderOptions));

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            using HttpClient client = new();

            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg-journal");

            logger.LogInformation("Downloading BAG file");

            using HttpResponseMessage response = await client.GetAsync(FileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            var destinationPath = Path.GetFileName(FileUrl);

            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await contentStream.CopyToAsync(fileStream, cancellationToken);
                await fileStream.FlushAsync(cancellationToken);
            }

            var fileInfo = new FileInfo(destinationPath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("Downloaded BAG file not found", fileInfo.Name);
            }

            if (fileInfo.Length < MinimumFileSize)
            {
                throw new InvalidOperationException("Downloaded BAG file is too small");
            }

            logger.LogInformation("Processing BAG file");

            await operationRepository.CleanupBAGAsync();

            var dataSourceBuilder = new Npgsql.NpgsqlConnectionStringBuilder(_dbProviderOptions.ConnectionString);
            var output = $"PG:dbname='{dataSourceBuilder.Database}' host='{dataSourceBuilder.Host}' port='{dataSourceBuilder.Port}' user='{dataSourceBuilder.Username}' password='{dataSourceBuilder.Password}'";
            gdalService.Convert(destinationPath, output);

            logger.LogInformation("Copying BAG file to building table");

            await operationRepository.LoadBuildingAsync();
            await operationRepository.LoadResidenceAsync();
            await operationRepository.LoadAddressAsync();

            // logger.LogInformation("Storing BAG file");

            // DateTime currentDate = DateTime.Now;
            // string dateString = currentDate.ToString("yyyy-MM-dd");

            // await blobStorageService.StoreFileAsync($"lvbag/archive/{dateString}/bag-light.gpkg", destinationPath);
        }
        finally
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg-journal");

            await operationRepository.CleanupBAGAsync();
        }
    }
}
