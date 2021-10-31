using System;
using System.Threading;
using System.Threading.Tasks;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace FunderMaps.AspNetCore.HealthChecks;

/// <summary>
///     Check if the blob storage backend is alive.
/// </summary>
public class BlobStorageHealthCheck : IHealthCheck
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<BlobStorageHealthCheck> _logger;

    /// <summary>
    ///     Create a new instance.
    /// </summary>
    public BlobStorageHealthCheck(IBlobStorageService blobStorageService, ILogger<BlobStorageHealthCheck> logger)
        => (_blobStorageService, _logger) = (blobStorageService, logger);

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
            await _blobStorageService.HealthCheck();
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            _logger.LogTrace(exception, "Health check failed");

            return HealthCheckResult.Unhealthy("blob storage service");
        }
    }
}
