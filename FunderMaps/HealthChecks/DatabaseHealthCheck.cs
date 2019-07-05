using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace FunderMaps.HealthChecks
{
    /// <summary>
    /// Check if the database is alive.
    /// </summary>
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public DatabaseHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns><see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("FunderMapsConnection")))
            {
                await connection.OpenAsync();
                switch (connection.State)
                {
                    case System.Data.ConnectionState.Broken:
                    case System.Data.ConnectionState.Closed:
                        return HealthCheckResult.Unhealthy();
                    case System.Data.ConnectionState.Connecting:
                        return HealthCheckResult.Degraded();
                    case System.Data.ConnectionState.Executing:
                    case System.Data.ConnectionState.Fetching:
                    case System.Data.ConnectionState.Open:
                        return HealthCheckResult.Healthy();
                }
            }

            return HealthCheckResult.Unhealthy();
        }
    }
}
