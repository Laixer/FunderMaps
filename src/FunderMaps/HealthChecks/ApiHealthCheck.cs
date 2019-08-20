using FunderMaps.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static FunderMaps.Controllers.Api.VersionController;

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
        public ApiHealthCheck(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns><see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            string myUrl = request.Scheme + "://" + request.Host.ToString() + "/api/version";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(myUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Unhealthy();
                }

                var version = await response.Content.ReadAsAsync<VersionOutputModel>();
                if (version.Name == Constants.ApplicationName && version.Version == Constants.ApplicationVersion)
                {
                    return HealthCheckResult.Healthy();
                }
            }

            return HealthCheckResult.Unhealthy();
        }
    }
}
