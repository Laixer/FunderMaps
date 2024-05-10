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
    IGDALService gdalService,
    IOperationRepository operationRepository,
    ILogger<LoadBagTask> logger) : ITaskService
{
    private const string FileUrl = "https://service.pdok.nl/lv/bag/atom/downloads/bag-light.gpkg";
    private const int MinimumFileSize = 1024 * 1024; // 1 MB

    private readonly DbProviderOptions _dbProviderOptions = dbProviderOptions?.Value ?? throw new ArgumentNullException(nameof(dbProviderOptions));

    private static async Task DownloadFileAsync(string url, string destinationPath, CancellationToken cancellationToken)
    {
        using HttpClient client = new();

        using HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await contentStream.CopyToAsync(fileStream, cancellationToken);
        await fileStream.FlushAsync(cancellationToken);
    }

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg-journal");

            var fileName = Path.GetFileName(FileUrl);

            if (!File.Exists(fileName))
            {
                logger.LogInformation("Downloading BAG file");

                await DownloadFileAsync(FileUrl, fileName, cancellationToken);
            }

            var fileInfo = new FileInfo(fileName);
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
            gdalService.Convert(fileName, output);

            logger.LogInformation("Copying BAG file to building table");

            await operationRepository.LoadBuildingAsync();
            await operationRepository.LoadResidenceAsync();
            await operationRepository.LoadAddressAsync();
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
