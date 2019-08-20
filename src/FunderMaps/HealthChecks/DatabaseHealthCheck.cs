using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FunderMaps.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FunderMaps.HealthChecks
{
    /// <summary>
    /// Check if the database is alive.
    /// </summary>
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly DbProvider _dbProvider;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public DatabaseHealthCheck(DbProvider dbprovider)
        {
            _dbProvider = dbprovider;
        }

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns><see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = _dbProvider.ConnectionScope())
            {
                return await connection.ExecuteScalarAsync<int>("SELECT 1") == 1
                    ? HealthCheckResult.Healthy()
                    : HealthCheckResult.Unhealthy();
            }
        }
    }
}
