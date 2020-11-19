using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.AspNetCore.HealthChecks
{
    /// <summary>
    ///     Check if the API is alive.
    /// </summary>
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        ///     Create a new instance.
        /// </summary>
        public ApiHealthCheck(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        /// <summary>
        ///     Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns>Instance of <see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            // Only check the API over a remote connection.
            if (request.Host.Host is "localhost" or "127.0.0.1" or "::")
            {
                return HealthCheckResult.Healthy();
            }

            using HttpClient client = new();
            using var response = await client.GetAsync(new Uri($"{request.Scheme}://{request.Host}/api/version"), cancellationToken);

            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
        }
    }
}
