using System.Threading;
using System.Threading.Tasks;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FunderMaps.AspNetCore.HealthChecks
{
    /// <summary>
    ///     Check if the batch backend is alive.
    /// </summary>
    public class BatchHealthCheck : IHealthCheck
    {
        private readonly IBatchService _batchService;

        /// <summary>
        ///     Create a new instance.
        /// </summary>
        public BatchHealthCheck(IBatchService batchService)
            => _batchService = batchService;

        /// <summary>
        ///     Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns>Instance of <see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            // Operation throws if an error occurs.
            await _batchService.TestService();
            return HealthCheckResult.Healthy();
        }
    }
}
