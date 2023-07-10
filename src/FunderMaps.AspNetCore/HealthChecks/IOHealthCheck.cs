using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FunderMaps.AspNetCore.HealthChecks;

/// <summary>
///     Check if filesystem is writable.
/// </summary>
public class IOHealthCheck : IHealthCheck
{
    /// <summary>
    ///     Runs the health check, returning the status of the component being checked.
    /// </summary>
    /// <param name="context">A context object associated with the current execution.</param>
    /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
    /// <returns>Instance of <see cref="HealthCheckResult"/>.</returns>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var fileName = Path.GetTempFileName();

        try
        {
            File.Create(fileName).Close();
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        finally
        {
            File.Delete(fileName);
        }
    }
}
