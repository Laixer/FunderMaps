using FunderMaps.Core;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Repository for analysis products.
/// </summary>
internal sealed class AnalysisRepository : DbServiceBase, IAnalysisRepository
{
    // FUTURE: Remove address_id and external_address_id
    // FUTURE: Return enforcement_term and overall_quality
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<AnalysisProduct> GetAsync(string id)
    {
        var sql = @"
            SELECT
                    mrs.building_id,
                    mrs.external_building_id,
                    a.id as address_id,
				    a.external_id as address_external_id,
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
                    --mrs.enforcement_term,
                    --mrs.overall_quality,
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
            JOIN    geocoder.address_building ab ON ab.building_id = mrs.building_id
            JOIN    geocoder.address a ON a.id = ab.address_id
            WHERE   mrs.building_id = @id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await using var reader = await context.ReaderAsync();

        return MapFromReader(reader);
    }

    /// <summary>
    ///     Gets the risk index by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<bool> GetRiskIndexAsync(string id)
    {
        var sql = @"
            SELECT exists
            (
                SELECT TRUE
                FROM data.model_risk_static mrs
                WHERE mrs.building_id = @id
                AND
                (
                    (mrs.drystand_risk IS NOT NULL AND mrs.drystand_risk <> 'a')
                    OR
                    (mrs.bio_infection_risk IS NOT NULL AND mrs.bio_infection_risk <> 'a')
                    OR
                    (mrs.dewatering_depth_risk IS NOT NULL AND mrs.dewatering_depth_risk <> 'a')
                    OR
                    (mrs.unclassified_risk IS NOT NULL AND mrs.unclassified_risk <> 'a')
                )
                LIMIT 1
            )
            LIMIT 1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        return await context.ScalarAsync<bool>();
    }

    /// <summary>
    ///     Register a product match.
    /// </summary>
    /// <param name="buildingId">Internal building id.</param>
    /// <param name="id">External identifier.</param>
    public async Task RegisterProductMatch(string buildingId, string id, string product)
    {
        var sql = @"
            INSERT INTO application.product_tracker(organization_id, product, building_id, identifier)
            SELECT @organization_id, @product, @building_id, @id
            WHERE NOT EXISTS (
                SELECT  1
                FROM    application.product_tracker pt
                WHERE   pt.organization_id = @organization_id
                AND     pt.product = @product
                AND     pt.identifier = @id
                AND     pt.create_date > CURRENT_TIMESTAMP - interval '24 hours'
            )";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("building_id", buildingId);
        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("product", product);
        context.AddParameterWithValue("organization_id", AppContext.OrganizationId);

        await context.NonQueryAsync(affectedGuard: false);
    }

    /// <summary>
    ///     Register a product mismatch.
    /// </summary>
    /// <param name="id">External identifier.</param>
    public async Task RegisterProductMismatch(string id)
    {
        var sql = @"
            INSERT INTO application.product_tracker_mismatch(organization_id, identifier)
            VALUES(@organization_id, @id)";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("organization_id", AppContext.OrganizationId);

        await context.NonQueryAsync(affectedGuard: false);
    }

    /// <summary>
    ///     Maps a reader to an <see cref="AnalysisProduct"/>.
    /// </summary>
    public static AnalysisProduct MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            BuildingId = reader.GetString(offset++),
            ExternalBuildingId = reader.GetString(offset++),
            AddressId = reader.GetString(offset++),
            ExternalAddressId = reader.GetString(offset++),
            NeighborhoodId = reader.GetString(offset++),
            ConstructionYear = reader.GetSafeInt(offset++),
            ConstructionYearReliability = reader.GetFieldValue<Reliability>(offset++),
            FoundationType = reader.GetFieldValue<FoundationType>(offset++),
            FoundationTypeReliability = reader.GetFieldValue<Reliability>(offset++),
            RestorationCosts = reader.GetSafeInt(offset++),
            Height = reader.GetSafeDouble(offset++),
            Velocity = reader.GetSafeDouble(offset++),
            GroundWaterLevel = reader.GetSafeDouble(offset++),
            GroundLevel = reader.GetSafeDouble(offset++),
            Soil = reader.GetSafeString(offset++),
            SurfaceArea = reader.GetSafeDouble(offset++),
            DamageCause = reader.GetFieldValue<FoundationDamageCause?>(offset++),
            InquiryType = reader.GetFieldValue<InquiryType?>(offset++),
            Drystand = reader.GetSafeDouble(offset++),
            DrystandRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
            DrystandReliability = reader.GetFieldValue<Reliability>(offset++),
            BioInfectionRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
            BioInfectionReliability = reader.GetFieldValue<Reliability>(offset++),
            DewateringDepth = reader.GetSafeDouble(offset++),
            DewateringDepthRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
            DewateringDepthReliability = reader.GetFieldValue<Reliability>(offset++),
            UnclassifiedRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
            RecoveryType = reader.GetFieldValue<RecoveryType?>(offset++),
        };
}
