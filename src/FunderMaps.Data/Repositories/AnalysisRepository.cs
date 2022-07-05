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
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<AnalysisProduct3> Get3Async(string id)
    {
        var sql = @"
            WITH tracker AS (
                INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
                SELECT  @tenant, 'analysis3', building_id
                FROM    geocoder.id_lookup(@id) AS building_id
                LIMIT   1
                RETURNING building_id
            )
            SELECT-- AnalysisComplete
                    ac.building_id,
                    ac.external_building_id,
                    ac.address_id,
                    ac.address_external_id,
                    ac.neighborhood_id,
                    ac.construction_year,
                    ac.construction_year_reliability,
                    ac.foundation_type,
                    ac.foundation_type_reliability,
                    ac.restoration_costs,
                    ac.height,
                    ac.velocity,
                    ac.ground_water_level,
                    ac.ground_level,
                    ac.soil,
                    ac.surface_area,
                    ac.damage_cause,
                    ac.enforcement_term,
                    ac.overall_quality,
                    ac.inquiry_type,
                    ac.drystand,
                    ac.drystand_risk,
                    ac.drystand_risk_reliability,
                    ac.bio_infection_risk,
                    ac.bio_infection_risk_reliability,
                    ac.dewatering_depth,
                    ac.dewatering_depth_risk,
                    ac.dewatering_depth_risk_reliability,
                    ac.unclassified_risk,
                    ac.recovery_type
            FROM    data.analysis_complete ac, tracker
            WHERE   ac.building_id = tracker.building_id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader3(reader);
    }

    // TODO: Needs optimization.
    /// <summary>
    ///     Gets the risk index by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<bool> GetRiskIndexAsync(string id)
    {
        var sql = @"
            WITH tracker AS (
                INSERT INTO application.product_tracker AS pt (organization_id, product, building_id)
                SELECT  @tenant, 'riskindex', building_id
                FROM    geocoder.id_lookup(@id) AS building_id
                LIMIT   1
                RETURNING building_id
            )
            SELECT -- AnalysisComplete
                    'a'::data.foundation_risk_indication <> ANY (ARRAY[
                    CASE
                            WHEN ac.drystand_risk IS NULL THEN 'a'::data.foundation_risk_indication
                            ELSE ac.drystand_risk
                    END,
                    CASE
                            WHEN ac.bio_infection_risk IS NULL THEN 'a'::data.foundation_risk_indication
                            ELSE ac.bio_infection_risk
                    END,
                    CASE
                            WHEN ac.dewatering_depth_risk IS NULL THEN 'a'::data.foundation_risk_indication
                            ELSE ac.dewatering_depth_risk
                    END,
                    CASE
                            WHEN ac.unclassified_risk IS NULL THEN 'a'::data.foundation_risk_indication
                            ELSE ac.unclassified_risk
                    END]) AS has_risk
            FROM    data.analysis_complete ac, tracker
            WHERE   ac.building_id = tracker.building_id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        return await context.ScalarAsync<bool>();
    }

    /// <summary>
    ///     Maps a reader to an <see cref="AnalysisProduct3"/>.
    /// </summary>
    public static AnalysisProduct3 MapFromReader3(DbDataReader reader)
        => new()
        {
            BuildingId = reader.GetSafeString(0),
            ExternalBuildingId = reader.GetSafeString(1),
            AddressId = reader.GetSafeString(2),
            ExternalAddressId = reader.GetSafeString(3),
            NeighborhoodId = reader.GetSafeString(4),
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
            // EnforcementTerm = reader.GetFieldValue<EnforcementTerm?>(17),
            // OverallQuality = reader.GetFieldValue<Quality?>(18),
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
