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
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    public async Task<AnalysisProduct> GetAsync(string id)
    {
        var sql = @"
            SELECT
                ra.building_id,
                ra.external_building_id,
                a.id as address_id,
                a.external_id as address_external_id,
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
            FROM application.request_analysis(@tenant, @id) ra
            JOIN geocoder.address_building ab ON ab.building_id = ra.building_id
            JOIN geocoder.address a ON a.id = ab.address_id
            LIMIT 1";

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
        var sql = @"SELECT application.request_risk_index(@tenant, @id)";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        return await context.ScalarAsync<bool>();
    }

    /// <summary>
    ///     Maps a reader to an <see cref="AnalysisProduct"/>.
    /// </summary>
    public static AnalysisProduct MapFromReader(DbDataReader reader)
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
