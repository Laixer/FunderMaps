using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Worker;

/// <summary>
///     Construct new instance.
/// </summary>
public class MapBundleTask(
    IOptions<DbProviderOptions> dbProviderOptions,
    IBlobStorageService blobStorageService,
    IGDALService gdalService,
    IBundleRepository bundleRepository,
    ITilesetGeneratorService tilesetGeneratorService,
    IMapboxService mapboxService,
    ILogger<MapBundleTask> logger) : ISingleShotTask
{
    private readonly DbProviderOptions _dbProviderOptions = dbProviderOptions?.Value ?? throw new ArgumentNullException(nameof(dbProviderOptions));
    private readonly IBlobStorageService _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
    private readonly IGDALService _gdalService = gdalService ?? throw new ArgumentNullException(nameof(gdalService));
    private readonly ITilesetGeneratorService _tilesetGeneratorService = tilesetGeneratorService ?? throw new ArgumentNullException(nameof(tilesetGeneratorService));
    private readonly IBundleRepository _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
    private readonly IMapboxService _mapboxService = mapboxService ?? throw new ArgumentNullException(nameof(mapboxService));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await foreach (var bundle in _bundleRepository.ListAllEnabledAsync())
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (!string.IsNullOrEmpty(bundle.Precondition))
                {
                    if (!await _bundleRepository.RunPreconditionAsync(bundle.Tileset, bundle.Precondition))
                    {
                        _logger.LogInformation("Precondition for bundle '{Tileset}' failed, skipping", bundle.Tileset);
                        continue;
                    }
                }

                _logger.LogInformation("Processing tileset '{Tileset}'", bundle.Tileset);

                var dataSourceBuilder = new Npgsql.NpgsqlConnectionStringBuilder(_dbProviderOptions.ConnectionString);
                var input = $"PG:dbname='{dataSourceBuilder.Database}' host='{dataSourceBuilder.Host}' port='{dataSourceBuilder.Port}' user='{dataSourceBuilder.Username}' password='{dataSourceBuilder.Password}'";

                _gdalService.Convert(input, $"{bundle.Tileset}.gpkg", $"maplayer.{bundle.Tileset}", cancellationToken);
                await _blobStorageService.StoreFileAsync($"tileset/{bundle.Tileset}.gpkg", $"{bundle.Tileset}.gpkg");

                if (bundle.MapEnabled)
                {
                    _logger.LogInformation("Generating map for tileset '{Tileset}'", bundle.Tileset);

                    _gdalService.Convert($"{bundle.Tileset}.gpkg", $"{bundle.Tileset}.geojson", cancellationToken: cancellationToken);
                    _tilesetGeneratorService.Generate($"{bundle.Tileset}.geojson", $"{bundle.Tileset}.mbtiles", bundle.Tileset, bundle.MaxZoomLevel, bundle.MinZoomLevel, cancellationToken);
                    await _mapboxService.UploadAsync(bundle.Name, bundle.Tileset, $"{bundle.Tileset}.mbtiles");
                }

                await _bundleRepository.LogBuiltTimeAsync(bundle.Tileset);
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
    }
}
