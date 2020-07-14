namespace FunderMaps.HealthChecks
{
#if DISABLED
    /// <summary>
    /// Check if the database is alive.
    /// </summary>
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly DbProvider _dbProvider;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public DatabaseHealthCheck(DbProvider dbProvider) => _dbProvider = dbProvider;

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns><see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            using var connection = _dbProvider.ConnectionScope();
            return await connection.ExecuteScalarAsync<int>("SELECT pg_backend_pid()") > 1
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
        }
    }
#endif
}
