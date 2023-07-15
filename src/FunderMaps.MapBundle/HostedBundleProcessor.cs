using FunderMaps.Core.Email;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.MapBundle;

public class HostedBundleProcessor : IHostedService
{
    private readonly HealthCheckService _healthCheckService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IEmailService _emailService;
    private readonly IGDALService _gdalService;
    private readonly ITilesetGeneratorService _tilesetGeneratorService;
    private readonly IMapboxService _mapboxService;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly DbProviderOptions _dbProviderOptions;
    private readonly ILogger<HostedBundleProcessor> _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public HostedBundleProcessor(
        HealthCheckService healthCheckService,
        IServiceScopeFactory serviceScopeFactory,
        IBlobStorageService blobStorageService,
        IEmailService emailService,
        IGDALService gdalService,
        ITilesetGeneratorService tilesetGeneratorService,
        IMapboxService mapboxService,
        IHostApplicationLifetime hostApplicationLifetime,
        IOptions<DbProviderOptions> dbProviderOptions,
        ILogger<HostedBundleProcessor> logger)
    {
        _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _gdalService = gdalService ?? throw new ArgumentNullException(nameof(gdalService));
        _tilesetGeneratorService = tilesetGeneratorService ?? throw new ArgumentNullException(nameof(tilesetGeneratorService));
        _mapboxService = mapboxService ?? throw new ArgumentNullException(nameof(mapboxService));
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _dbProviderOptions = dbProviderOptions?.Value ?? throw new ArgumentNullException(nameof(dbProviderOptions));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            if (healthReport.Status != HealthStatus.Healthy)
            {
                _logger.LogError("Health check failed, stopping application");

                throw new InvalidOperationException("Health check failed, stopping application");
            }
            else
            {
                using var scope = _serviceScopeFactory.CreateScope();

                await RunAllEnabledAsync(scope, cancellationToken);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while processing bundles");

            await _emailService.SendAsync(new EmailMessage
            {
                ToAddresses = new[]
                {
                    // TODO: Make configurable
                    new EmailAddress("yorick@laixer.com", "Yorick de Wid")
                },
                Subject = "Error while processing bundles",
                Content = "Error while processing bundles, see log for details.",
            });
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }

    /// <summary>
    ///     Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task RunAllEnabledAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        var bundleRepository = scope.ServiceProvider.GetRequiredService<IBundleRepository>();

        await foreach (var bundle in bundleRepository.ListAllEnabledAsync())
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (!string.IsNullOrEmpty(bundle.Precondition))
                {
                    if (!await bundleRepository.RunPreconditionAsync(bundle.Tileset, bundle.Precondition))
                    {
                        _logger.LogInformation($"Precondition for bundle '{bundle.Tileset}' failed, skipping");

                        continue;
                    }
                }

                _logger.LogInformation($"Processing bundle '{bundle.Tileset}'");

                var dataSourceBuilder = new Npgsql.NpgsqlConnectionStringBuilder(_dbProviderOptions.ConnectionString);

                var input = $"PG:dbname='{dataSourceBuilder.Database}' host='{dataSourceBuilder.Host}' port='{dataSourceBuilder.Port}' user='{dataSourceBuilder.Username}' password='{dataSourceBuilder.Password}'";

                _gdalService.Convert(input, $"{bundle.Tileset}.gpkg", $"maplayer.{bundle.Tileset}");

                await _blobStorageService.StoreFileAsync($"tileset/{bundle.Tileset}.gpkg", $"{bundle.Tileset}.gpkg");

                _gdalService.Convert($"{bundle.Tileset}.gpkg", $"{bundle.Tileset}.geojson");

                _tilesetGeneratorService.Generate($"{bundle.Tileset}.geojson", $"{bundle.Tileset}.mbtiles", bundle.Tileset, bundle.MaxZoomLevel, bundle.MinZoomLevel);

                await _mapboxService.UploadAsync(bundle.Name, bundle.Tileset, $"{bundle.Tileset}.mbtiles");

                await bundleRepository.LogBuiltTimeAsync(bundle.Tileset);
            }
            finally
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                foreach (string file in Directory.GetFiles(currentDirectory, "*.gpkg"))
                {
                    File.Delete(file);
                }
                foreach (string file in Directory.GetFiles(currentDirectory, "*.gpkg-journal"))
                {
                    File.Delete(file);
                }
                foreach (string file in Directory.GetFiles(currentDirectory, "*.geojson"))
                {
                    File.Delete(file);
                }
                foreach (string file in Directory.GetFiles(currentDirectory, "*.mbtiles"))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
