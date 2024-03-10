using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.HealthChecks;

/// <summary>
///     Check if the tile generator backend is alive.
/// </summary>
public class TippecanoeHealthCheck(ITilesetGeneratorService tilesetGeneratorService, ILogger<TippecanoeHealthCheck> logger) : IHealthCheck
{
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
            await tilesetGeneratorService.HealthCheck();
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            logger.LogTrace(exception, "Health check failed");

            return HealthCheckResult.Unhealthy("tileset generator is unhealthy");
        }
    }
}
