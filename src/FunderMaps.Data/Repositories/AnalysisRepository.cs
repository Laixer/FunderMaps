using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace FunderMaps.Data.Repositories;

internal sealed class AnalysisRepository : DbServiceBase, IAnalysisRepository
{
    // FUTURE: Add owner, address_count
    public async Task<AnalysisProduct> GetAsync(string id)
    {
        if (Cache.TryGetValue(id, out AnalysisProduct? value))
        {
            return value ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT
                    mrs.building_id,
                    mrs.external_building_id,
                    mrs.neighborhood_id,
                    mrs.construction_year,
                    mrs.construction_year_reliability,
                    mrs.foundation_type,
                    mrs.foundation_type_reliability,
                    mrs.restoration_costs,
                    mrs.height,
                    mrs.velocity,
                    mrs.ground_water_level,
                    mrs.ground_level,
                    mrs.soil,
                    mrs.surface_area,
                    mrs.damage_cause,
                    mrs.inquiry_type,
                    mrs.drystand,
                    mrs.drystand_risk,
                    mrs.drystand_risk_reliability,
                    mrs.bio_infection_risk,
                    mrs.bio_infection_risk_reliability,
                    mrs.dewatering_depth,
                    mrs.dewatering_depth_risk,
                    mrs.dewatering_depth_risk_reliability,
                    mrs.unclassified_risk,
                    mrs.recovery_type
            FROM    data.model_risk_static mrs
            WHERE   mrs.building_id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var analysis = await connection.QuerySingleOrDefaultAsync<AnalysisProduct>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(AnalysisProduct));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

        return Cache.Set(id, analysis, options);
    }

    public async Task<bool> RegisterProductMatch(string buildingId, string id, string product, Guid tenantId)
    {
        var sql = @"
            WITH register_product_request AS (
                INSERT INTO application.product_tracker(organization_id, product, building_id, identifier)
                SELECT @organization_id, @product, @building_id, @id
                WHERE NOT EXISTS (
                    SELECT  1
                    FROM    application.product_tracker pt
                    WHERE   pt.organization_id = @organization_id
                    AND     pt.product = @product
                    AND     pt.identifier = @id
                    AND     pt.create_date > CURRENT_TIMESTAMP - interval '24 hours'
                )
                RETURNING 1
            )
            SELECT EXISTS (SELECT 1 FROM register_product_request) AS is_registered";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<bool>(sql, new { building_id = buildingId, id, product, organization_id = tenantId });
    }

    public async Task RegisterProductMismatch(string id, Guid tenantId)
    {
        var sql = @"
            INSERT INTO application.product_tracker_mismatch(organization_id, identifier)
            VALUES(@organization_id, @id)";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id, organization_id = tenantId });
    }

    public async IAsyncEnumerable<AnalysisProduct> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT
                    mrs.building_id,
                    mrs.external_building_id,
                    mrs.neighborhood_id,
                    mrs.construction_year,
                    mrs.construction_year_reliability,
                    mrs.foundation_type,
                    mrs.foundation_type_reliability,
                    mrs.restoration_costs,
                    mrs.height,
                    mrs.velocity,
                    mrs.ground_water_level,
                    mrs.ground_level,
                    mrs.soil,
                    mrs.surface_area,
                    mrs.damage_cause,
                    mrs.enforcement_term,
                    mrs.overall_quality,
                    mrs.inquiry_type,
                    mrs.drystand,
                    mrs.drystand_risk,
                    mrs.drystand_risk_reliability,
                    mrs.bio_infection_risk,
                    mrs.bio_infection_risk_reliability,
                    mrs.dewatering_depth,
                    mrs.dewatering_depth_risk,
                    mrs.dewatering_depth_risk_reliability,
                    mrs.unclassified_risk,
                    mrs.recovery_type
            FROM    data.model_risk_static mrs";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<AnalysisProduct>(sql, navigation))
        {
            yield return item;
        }
    }
}
