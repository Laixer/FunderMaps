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
            SELECT
                ra.building_id,
                ra.external_building_id,
                null as address_id,
                null as address_external_id,
                ra.neighborhood_id,
                ra.construction_year,
                ra.construction_year_reliability,
                ra.foundation_type,
                ra.foundation_type_reliability,
                ra.restoration_costs,
                ra.height,
                ra.velocity,
                ra.ground_water_level,
                ra.ground_level,
                ra.soil,
                ra.surface_area,
                ra.damage_cause,
                ra.enforcement_term,
                ra.overall_quality,
                ra.inquiry_type,
                ra.drystand,
                ra.drystand_risk,
                ra.drystand_risk_reliability,
                ra.bio_infection_risk,
                ra.bio_infection_risk_reliability,
                ra.dewatering_depth,
                ra.dewatering_depth_risk,
                ra.dewatering_depth_risk_reliability,
                ra.unclassified_risk,
                ra.recovery_type
            FROM application.request_analysis(@tenant, @id) ra";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader3(reader);
    }

    // TOOD: Move to db.
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
