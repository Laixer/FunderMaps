using Dapper;
using FunderMaps.Data.Providers;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FunderMaps.Data.Extensions;

internal sealed class DatabaseHealthCheck(DbProvider dbProvider) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        await using var connection = dbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<int>("SELECT 1") == 1
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }
}
