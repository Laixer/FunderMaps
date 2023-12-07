using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Data.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Worker;

/// <summary>
///     Construct new instance.
/// </summary>
public class DownloadBagTask(
    IOptions<DbProviderOptions> dbProviderOptions,
    IGDALService gdalService,
    ILogger<DownloadBagTask> logger) : ISingleShotTask
{
    private const string FileUrl = "https://service.pdok.nl/lv/bag/atom/downloads/bag-light.gpkg";

    private readonly DbProviderOptions _dbProviderOptions = dbProviderOptions?.Value ?? throw new ArgumentNullException(nameof(dbProviderOptions));
    private readonly IGDALService _gdalService = gdalService ?? throw new ArgumentNullException(nameof(gdalService));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            using HttpClient client = new();

            _logger.LogInformation("Downloading BAG file");

            using HttpResponseMessage response = await client.GetAsync(FileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            var destinationPath = Path.GetFileName(FileUrl);

            using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await contentStream.CopyToAsync(fileStream, cancellationToken);
            await fileStream.FlushAsync(cancellationToken);

            _logger.LogInformation("Processing BAG file");

            var dataSourceBuilder = new Npgsql.NpgsqlConnectionStringBuilder(_dbProviderOptions.ConnectionString);
            var output = $"PG:dbname='{dataSourceBuilder.Database}' host='{dataSourceBuilder.Host}' port='{dataSourceBuilder.Port}' user='{dataSourceBuilder.Username}' password='{dataSourceBuilder.Password}'";
            _gdalService.Convert(destinationPath, output, cancellationToken: cancellationToken);
        }
        finally
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg-journal");
        }
    }
}
