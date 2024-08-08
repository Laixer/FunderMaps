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

        // return analysis;

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", id);

        // await using var reader = await context.ReaderAsync();

        // return MapFromReader(reader);
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

    // private static AnalysisProduct MapFromReader(DbDataReader reader, int offset = 0)
    //     => new()
    //     {
    //         BuildingId = reader.GetString(offset++),
    //         ExternalBuildingId = reader.GetString(offset++),
    //         NeighborhoodId = reader.GetSafeString(offset++),
    //         ConstructionYear = reader.GetSafeInt(offset++),
    //         ConstructionYearReliability = reader.GetFieldValue<Reliability>(offset++),
    //         FoundationType = reader.GetFieldValue<FoundationType>(offset++),
    //         FoundationTypeReliability = reader.GetFieldValue<Reliability>(offset++),
    //         RestorationCosts = reader.GetSafeInt(offset++),
    //         Height = reader.GetSafeDouble(offset++),
    //         Velocity = reader.GetSafeDouble(offset++),
    //         GroundWaterLevel = reader.GetSafeDouble(offset++),
    //         GroundLevel = reader.GetSafeDouble(offset++),
    //         Soil = reader.GetSafeString(offset++),
    //         SurfaceArea = reader.GetSafeDouble(offset++),
    //         DamageCause = reader.GetFieldValue<FoundationDamageCause?>(offset++),
    //         InquiryType = reader.GetFieldValue<InquiryType?>(offset++),
    //         Drystand = reader.GetSafeDouble(offset++),
    //         DrystandRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
    //         DrystandReliability = reader.GetFieldValue<Reliability>(offset++),
    //         BioInfectionRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
    //         BioInfectionReliability = reader.GetFieldValue<Reliability>(offset++),
    //         DewateringDepth = reader.GetSafeDouble(offset++),
    //         DewateringDepthRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
    //         DewateringDepthReliability = reader.GetFieldValue<Reliability>(offset++),
    //         UnclassifiedRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
    //         RecoveryType = reader.GetFieldValue<RecoveryType?>(offset++),
    //     };

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
