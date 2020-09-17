using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.HealthChecks
{
    /// <summary>
    /// Check if the API is alive.
    /// </summary>
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="httpContextAccessor">HttpContext container.</param>
        public ApiHealthCheck(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns><see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            using var client = new HttpClient();
            using var response = await client.GetAsync(new Uri($"https://{request.Host}/api/version"), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Unhealthy();
            }

            //var version = await response.Content.ReadAsAsync<ApplicationVersionModel>();
            //if (version.Name == Constants.ApplicationName && version.Version == Constants.ApplicationVersion)
            //{
            return HealthCheckResult.Healthy();
            //}

            //return HealthCheckResult.Unhealthy();
        }
    }
}
