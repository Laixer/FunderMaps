using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FunderMaps.Core.HealthChecks;

/// <summary>
///     Check if the data backend is alive.
/// </summary>
public class RepositoryHealthCheck(ITestRepository testRepository) : IHealthCheck
{
    /// <summary>
    ///     Runs the health check, returning the status of the component being checked.
    /// </summary>
    /// <param name="context">A context object associated with the current execution.</param>
    /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
    /// <returns>Instance of <see cref="HealthCheckResult"/>.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        => await testRepository.IsAliveAsync()
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
}
