using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FunderMaps.MapBundle;

public class BundleProcessor
{
    private readonly IBundleRepository _bundleRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IGDALService _gdalService;
    private readonly ITilesetGeneratorService _tilesetGeneratorService;
    private readonly IMapboxService _mapboxService;
    private readonly ILogger<BundleProcessor> _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public BundleProcessor(
        IBundleRepository bundleRepository,
        IBlobStorageService blobStorageService,
        IGDALService gdalService,
        ITilesetGeneratorService tilesetGeneratorService,
        IMapboxService mapboxService,
        ILogger<BundleProcessor> logger)
    {
        _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        _gdalService = gdalService ?? throw new ArgumentNullException(nameof(gdalService));
        _tilesetGeneratorService = tilesetGeneratorService ?? throw new ArgumentNullException(nameof(tilesetGeneratorService));
        _mapboxService = mapboxService ?? throw new ArgumentNullException(nameof(mapboxService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task RunAsync()
    {
        await foreach (var bundle in _bundleRepository.ListAllEnabledAsync())
        {
            try
            {
                if (!string.IsNullOrEmpty(bundle.Precondition))
                {
                    if (!await _bundleRepository.RunPreconditionAsync(bundle.Tileset, bundle.Precondition))
                    {
                        _logger.LogInformation($"Precondition for bundle '{bundle.Tileset}' failed, skipping");

                        continue;
                    }
                }

                _logger.LogInformation($"Processing bundle '{bundle.Tileset}'");

                var input = "PG:dbname='fundermaps'";

                _gdalService.Convert(input, $"{bundle.Tileset}.gpkg", $"maplayer.{bundle.Tileset}");

                await _blobStorageService.StoreFileAsync($"tileset/{bundle.Tileset}.gpkg", $"{bundle.Tileset}.gpkg");

                _gdalService.Convert($"{bundle.Tileset}.gpkg", $"{bundle.Tileset}.geojson");

                _tilesetGeneratorService.Generate($"{bundle.Tileset}.geojson", $"{bundle.Tileset}.mbtiles", bundle.Tileset, bundle.MaxZoomLevel, bundle.MinZoomLevel);

                await _mapboxService.UploadAsync(bundle.Name, bundle.Tileset, $"{bundle.Tileset}.mbtiles");

                await _bundleRepository.LogBuiltTimeAsync(bundle.Tileset);
            }
            finally
            {
                if (File.Exists($"{bundle.Tileset}.gpkg"))
                {
                    File.Delete($"{bundle.Tileset}.gpkg");
                }
                if (File.Exists($"{bundle.Tileset}.geojson"))
                {
                    File.Delete($"{bundle.Tileset}.geojson");
                }
                if (File.Exists($"{bundle.Tileset}.mbtiles"))
                {
                    File.Delete($"{bundle.Tileset}.mbtiles");
                }
            }
        }
    }
}
