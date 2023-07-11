using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace FunderMaps.AspNetCore.HealthChecks;

/// <summary>
///     Check if the blob storage backend is alive.
/// </summary>
public class MapboxHealthCheck : IHealthCheck
{
    private readonly IMapboxService _mapboxService;
    private readonly ILogger<MapboxHealthCheck> _logger;

    /// <summary>
    ///     Create a new instance.
    /// </summary>
    public MapboxHealthCheck(IMapboxService mapboxService, ILogger<MapboxHealthCheck> logger)
        => (_mapboxService, _logger) = (mapboxService, logger);

    /// <summary>
    ///     Runs the health check, returning the status of the component being checked.
    /// </summary>
    /// <param name="context">A context object associated with the current execution.</param>
    /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
    /// <returns>Instance of <see cref="HealthCheckResult"/>.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        try
        {
            await _mapboxService.HealthCheck();
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            _logger.LogTrace(exception, "Health check failed");

            return HealthCheckResult.Unhealthy("mapbox service");
        }
    }
}
