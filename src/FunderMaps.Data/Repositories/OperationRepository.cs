using Dapper;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Various data operations.
/// </summary>
internal sealed class OperationRepository : DbServiceBase, IOperationRepository
{
    /// <summary>
    ///     Check if backend is online.
    /// </summary>
    /// <remarks>
    ///     Explicit check on result, not all commands are submitted
    ///     to the database.
    /// </remarks>
    public async Task<bool> IsAliveAsync()
    {
        var sql = @"SELECT 1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<int>(sql) == 1;
    }

    /// <summary>
    ///    Refresh data models.
    /// </summary>
    public async Task RefreshModelAsync()
    {
        var sql = @"
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.building_sample;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.cluster_sample;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.supercluster_sample;

            CALL data.model_risk_manifest();";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, commandTimeout: 10800);
    }

    /// <summary>
    ///   Refresh statistics.
    /// </summary>
    public async Task RefreshStatisticsAsync()
    {
        var sql = @"
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_inquiries;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_inquiry_municipality;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_incidents;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_incident_municipality;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_foundation_type;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_foundation_risk;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_data_collected;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_construction_years;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_product_buildings_restored;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_postal_code_foundation_type;
            REFRESH MATERIALIZED VIEW CONCURRENTLY data.statistics_postal_code_foundation_risk;";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, commandTimeout: 3600);
    }
}
