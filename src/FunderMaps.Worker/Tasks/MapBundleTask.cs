using FunderMaps.Core.Entities;
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
internal sealed class MapBundleTask(
    IOptions<DbProviderOptions> dbProviderOptions,
    IBlobStorageService blobStorageService,
    IGDALService gdalService,
    IBundleRepository bundleRepository,
    ITilesetGeneratorService tilesetGeneratorService,
    IMapboxService mapboxService,
    ILogger<MapBundleTask> logger) : ITaskService
{
    private readonly DbProviderOptions _dbProviderOptions = dbProviderOptions?.Value ?? throw new ArgumentNullException(nameof(dbProviderOptions));

    private async Task GenerateBundleAsync(Bundle bundle, CancellationToken cancellationToken)
    {
        try
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (!string.IsNullOrEmpty(bundle.Precondition))
            {
                if (!await bundleRepository.RunPreconditionAsync(bundle.Tileset, bundle.Precondition))
                {
                    logger.LogInformation("Precondition for bundle '{Tileset}' failed, skipping", bundle.Tileset);
                    return;
                }
            }

            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg-journal");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "geojson");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "mbtiles");

            logger.LogInformation("Processing tileset '{Tileset}'", bundle.Tileset);

            var dataSourceBuilder = new Npgsql.NpgsqlConnectionStringBuilder(_dbProviderOptions.ConnectionString);
            var input = $"PG:dbname='{dataSourceBuilder.Database}' host='{dataSourceBuilder.Host}' port='{dataSourceBuilder.Port}' user='{dataSourceBuilder.Username}' password='{dataSourceBuilder.Password}'";

            gdalService.Convert(input, $"{bundle.Tileset}.gpkg", $"maplayer.{bundle.Tileset}");
            await blobStorageService.StoreFileAsync($"tileset/{bundle.Tileset}.gpkg", $"{bundle.Tileset}.gpkg");

            DateTime currentDate = DateTime.Now;
            string dateString = currentDate.ToString("yyyy-MM-dd");

            await blobStorageService.StoreFileAsync($"tileset/archive/{dateString}/{bundle.Tileset}.gpkg", $"{bundle.Tileset}.gpkg");

            if (bundle.MapEnabled)
            {
                logger.LogInformation("Generating map for tileset '{Tileset}'", bundle.Tileset);

                gdalService.Convert($"{bundle.Tileset}.gpkg", $"{bundle.Tileset}.geojson");
                tilesetGeneratorService.Generate($"{bundle.Tileset}.geojson", $"{bundle.Tileset}.mbtiles", bundle.Tileset, bundle.MaxZoomLevel, bundle.MinZoomLevel, cancellationToken);
                await mapboxService.UploadAsync(bundle.Name, bundle.Tileset, $"{bundle.Tileset}.mbtiles");
            }

            await bundleRepository.LogBuiltTimeAsync(bundle.Tileset);
        }
        finally
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg-journal");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "geojson");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "mbtiles");
        }
    }

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await foreach (var bundle in bundleRepository.ListAllEnabledAsync())
        {
            await GenerateBundleAsync(bundle, cancellationToken);
        }
    }
}
