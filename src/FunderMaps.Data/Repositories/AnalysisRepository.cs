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
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<AnalysisProduct> GetAsync(string id)
    {
        // var sql = @"
        //     SELECT
        //         ra.building_id,
        //         ra.external_building_id,
        //         a.id as address_id,
        //         a.external_id as address_external_id,
        //         ra.neighborhood_id,
        //         ra.construction_year,
        //         ra.construction_year_reliability,
        //         ra.foundation_type,
        //         ra.foundation_type_reliability,
        //         ra.restoration_costs,
        //         ra.height,
        //         ra.velocity,
        //         ra.ground_water_level,
        //         ra.ground_level,
        //         ra.soil,
        //         ra.surface_area,
        //         ra.damage_cause,
        //         ra.enforcement_term,
        //         ra.overall_quality,
        //         ra.inquiry_type,
        //         ra.drystand,
        //         ra.drystand_risk,
        //         ra.drystand_risk_reliability,
        //         ra.bio_infection_risk,
        //         ra.bio_infection_risk_reliability,
        //         ra.dewatering_depth,
        //         ra.dewatering_depth_risk,
        //         ra.dewatering_depth_risk_reliability,
        //         ra.unclassified_risk,
        //         ra.recovery_type
        //     FROM application.request_analysis2(@organization_id, @id) ra
        //     JOIN geocoder.address_building ab ON ab.building_id = ra.building_id
        //     JOIN geocoder.address a ON a.id = ab.address_id
        //     LIMIT 1";

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
            FROM    data.model_risk_static mrs
            JOIN    geocoder.address_building ab ON ab.building_id = mrs.building_id
            JOIN    geocoder.address a ON a.id = ab.address_id
            WHERE   mrs.building_id = @id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        // context.AddParameterWithValue("organization_id", AppContext.OrganizationId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader(reader);
    }

    /// <summary>
    ///     Gets the risk index by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<bool> GetRiskIndexAsync(string id)
    {
        // var sql = @"SELECT application.request_risk_index(@organization_id, @id)";
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
        // context.AddParameterWithValue("organization_id", AppContext.OrganizationId);

        return await context.ScalarAsync<bool>();
    }

    public async Task RegisterAccess(string buildingId, string id, string product)
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

    public async Task RegisterMismatch(string id)
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
    public static AnalysisProduct MapFromReader(DbDataReader reader)
        => new()
        {
            BuildingId = reader.GetString(0),
            ExternalBuildingId = reader.GetString(1),
            AddressId = reader.GetString(2),
            ExternalAddressId = reader.GetString(3),
            NeighborhoodId = reader.GetString(4),
            ConstructionYear = reader.GetSafeInt(5),
            ConstructionYearReliability = reader.GetFieldValue<Reliability>(6),
            FoundationType = reader.GetFieldValue<FoundationType>(7),
            FoundationTypeReliability = reader.GetFieldValue<Reliability>(8),
            RestorationCosts = reader.GetSafeInt(9),
            Height = reader.GetSafeDouble(10),
            Velocity = reader.GetSafeDouble(11),
            GroundWaterLevel = reader.GetSafeDouble(12),
            GroundLevel = reader.GetSafeDouble(13),
            Soil = reader.GetSafeString(14),
            SurfaceArea = reader.GetSafeDouble(15),
            DamageCause = reader.GetFieldValue<FoundationDamageCause?>(16),
            InquiryType = reader.GetFieldValue<InquiryType?>(19),
            Drystand = reader.GetSafeDouble(20),
            DrystandRisk = reader.GetFieldValue<FoundationRisk?>(21),
            DrystandReliability = reader.GetFieldValue<Reliability>(22),
            BioInfectionRisk = reader.GetFieldValue<FoundationRisk?>(23),
            BioInfectionReliability = reader.GetFieldValue<Reliability>(24),
            DewateringDepth = reader.GetSafeDouble(25),
            DewateringDepthRisk = reader.GetFieldValue<FoundationRisk?>(26),
            DewateringDepthReliability = reader.GetFieldValue<Reliability>(27),
            UnclassifiedRisk = reader.GetFieldValue<FoundationRisk?>(28),
            RecoveryType = reader.GetFieldValue<RecoveryType?>(29),
        };
}
