using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Inquiry sample repository.
/// </summary>
internal class InquirySampleRepository : RepositoryBase<InquirySample, int>, IInquirySampleRepository
{
    /// <summary>
    ///     Create new <see cref="InquirySample"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    /// <returns>Created <see cref="InquirySample"/>.</returns>
    public override async Task<int> AddAsync(InquirySample entity)
    {
        var sql = @"
            INSERT INTO report.inquiry_sample(
                inquiry,
                address,
                building,
                note,
                built_year,
                substructure,
                overall_quality,
                wood_quality,
                construction_quality,
                wood_capacity_horizontal_quality,
                pile_wood_capacity_vertical_quality,
                carrying_capacity_quality,
                mason_quality,
                wood_quality_necessity,
                construction_level,
                wood_level,
                pile_diameter_top,
                pile_diameter_bottom,
                pile_head_level,
                pile_tip_level,
                foundation_depth,
                mason_level,
                concrete_charger_length,
                pile_distance_length,
                wood_penetration_depth,
                cpt,
                monitoring_well,
                groundwater_level_temp,
                groundlevel,
                groundwater_level_net,
                foundation_type,
                enforcement_term,
                recovery_advised,
                damage_cause,
                damage_characteristics,
                construction_pile,
                wood_type,
                wood_encroachement,
                crack_indoor_restored,
                crack_indoor_type,
                crack_indoor_size,
                crack_facade_front_restored,
                crack_facade_front_type,
                crack_facade_front_size,
                crack_facade_back_restored,
                crack_facade_back_type,
                crack_facade_back_size,
                crack_facade_left_restored,
                crack_facade_left_type,
                crack_facade_left_size,
                crack_facade_right_restored,
                crack_facade_right_type,
                crack_facade_right_size,
                deformed_facade,
                threshold_updown_skewed,
                threshold_front_level,
                threshold_back_level,
                skewed_parallel,
                skewed_parallel_facade,
                skewed_perpendicular,
                skewed_perpendicular_facade,
                settlement_speed,
                skewed_window_frame,
                facade_scan_risk)
            VALUES (
                @inquiry,
                @address,
                @building,
                NULLIF(trim(@note), ''),
                @built_year,
                @substructure,
                @overall_quality,
                @wood_quality,
                @construction_quality,
                @wood_capacity_horizontal_quality,
                @pile_wood_capacity_vertical_quality,
                @carrying_capacity_quality,
                @mason_quality,
                @wood_quality_necessity,
                @construction_level,
                @wood_level,
                @pile_diameter_top,
                @pile_diameter_bottom,
                @pile_head_level,
                @pile_tip_level,
                @foundation_depth,
                @mason_level,
                @concrete_charger_length,
                @pile_distance_length,
                @wood_penetration_depth,
                NULLIF(trim(@cpt), ''),
                NULLIF(trim(@monitoring_well), ''),
                @groundwater_level_temp,
                @groundlevel,
                @groundwater_level_net,
                @foundation_type,
                @enforcement_term,
                @recovery_advised,
                @damage_cause,
                @damage_characteristics,
                @construction_pile,
                @wood_type,
                @wood_encroachement,
                @crack_indoor_restored,
                @crack_indoor_type,
                @crack_indoor_size,
                @crack_facade_front_restored,
                @crack_facade_front_type,
                @crack_facade_front_size,
                @crack_facade_back_restored,
                @crack_facade_back_type,
                @crack_facade_back_size,
                @crack_facade_left_restored,
                @crack_facade_left_type,
                @crack_facade_left_size,
                @crack_facade_right_restored,
                @crack_facade_right_type,
                @crack_facade_right_size,
                @deformed_facade,
                @threshold_updown_skewed,
                @threshold_front_level,
                @threshold_back_level,
                @skewed_parallel,
                @skewed_parallel_facade,
                @skewed_perpendicular,
                @skewed_perpendicular_facade,
                @settlement_speed,
                @skewed_window_frame,
                @facade_scan_risk)
            RETURNING id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<int>(sql, entity);

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // MapToWriter(context, entity);

        // return await context.ScalarAsync<int>();
    }

    // TODO: Maybe remove this method?.
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public async Task<long> CountAsync(Guid tenantId)
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            WHERE   a.owner = @tenant";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql, new { tenant = tenantId });

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("tenant", tenantId);

        // return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public async Task<long> CountAsync(int report, Guid tenantId)
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.inquiry_sample AS s
            JOIN    report.inquiry AS i ON i.id = s.inquiry
            JOIN    application.attribution AS a ON a.id = i.attribution
            WHERE   a.owner = @tenant
            AND     i.id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql, new { id = report, tenant = tenantId });

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", report);
        // context.AddParameterWithValue("tenant", tenantId);

        // return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Delete <see cref="InquirySample"/>.
    /// </summary>
    /// <param name="id">Entity object.</param>
    public async Task DeleteAsync(int id, Guid tenantId)
    {
        ResetCacheEntity(id);

        var sql = @"
            DELETE
            FROM    report.inquiry_sample AS s
            USING 	application.attribution AS a, report.inquiry AS i
            WHERE   i.id = s.inquiry
            AND     a.id = i.attribution
            AND     s.id = @id
            AND     a.owner = @tenant";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id, tenant = tenantId });

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", id);
        // context.AddParameterWithValue("tenant", tenantId);

        // await context.NonQueryAsync();
    }

    private static void MapToWriter(DbContext context, InquirySample entity)
    {
        context.AddParameterWithValue("inquiry", entity.Inquiry);
        context.AddParameterWithValue("address", entity.Address);
        context.AddParameterWithValue("building", entity.Building);
        context.AddParameterWithValue("note", entity.Note);
        context.AddParameterWithValue("built_year", entity.BuiltYear);
        context.AddParameterWithValue("substructure", entity.Substructure);
        context.AddParameterWithValue("overall_quality", entity.OverallQuality);
        context.AddParameterWithValue("wood_quality", entity.WoodQuality);
        context.AddParameterWithValue("construction_quality", entity.ConstructionQuality);
        context.AddParameterWithValue("wood_capacity_horizontal_quality", entity.WoodCapacityHorizontalQuality);
        context.AddParameterWithValue("pile_wood_capacity_vertical_quality", entity.PileWoodCapacityVerticalQuality);
        context.AddParameterWithValue("carrying_capacity_quality", entity.CarryingCapacityQuality);
        context.AddParameterWithValue("mason_quality", entity.MasonQuality);
        context.AddParameterWithValue("wood_quality_necessity", entity.WoodQualityNecessity);
        context.AddParameterWithValue("construction_level", entity.ConstructionLevel);
        context.AddParameterWithValue("wood_level", entity.WoodLevel);
        context.AddParameterWithValue("pile_diameter_top", entity.PileDiameterTop);
        context.AddParameterWithValue("pile_diameter_bottom", entity.PileDiameterBottom);
        context.AddParameterWithValue("pile_head_level", entity.PileHeadLevel);
        context.AddParameterWithValue("pile_tip_level", entity.PileTipLevel);
        context.AddParameterWithValue("foundation_depth", entity.FoundationDepth);
        context.AddParameterWithValue("mason_level", entity.MasonLevel);
        context.AddParameterWithValue("concrete_charger_length", entity.ConcreteChargerLength);
        context.AddParameterWithValue("pile_distance_length", entity.PileDistanceLength);
        context.AddParameterWithValue("wood_penetration_depth", entity.WoodPenetrationDepth);
        context.AddParameterWithValue("cpt", entity.Cpt);
        context.AddParameterWithValue("monitoring_well", entity.MonitoringWell);
        context.AddParameterWithValue("groundwater_level_temp", entity.GroundwaterLevelTemp);
        context.AddParameterWithValue("groundlevel", entity.GroundLevel);
        context.AddParameterWithValue("groundwater_level_net", entity.GroundwaterLevelNet);
        context.AddParameterWithValue("foundation_type", entity.FoundationType);
        context.AddParameterWithValue("enforcement_term", entity.EnforcementTerm);
        context.AddParameterWithValue("recovery_advised", entity.RecoveryAdvised);
        context.AddParameterWithValue("damage_cause", entity.DamageCause);
        context.AddParameterWithValue("damage_characteristics", entity.DamageCharacteristics);
        context.AddParameterWithValue("construction_pile", entity.ConstructionPile);
        context.AddParameterWithValue("wood_type", entity.WoodType);
        context.AddParameterWithValue("wood_encroachement", entity.WoodEncroachement);
        context.AddParameterWithValue("crack_indoor_restored", entity.CrackIndoorRestored);
        context.AddParameterWithValue("crack_indoor_type", entity.CrackIndoorType);
        context.AddParameterWithValue("crack_indoor_size", entity.CrackIndoorSize);
        context.AddParameterWithValue("crack_facade_front_restored", entity.CrackFacadeFrontRestored);
        context.AddParameterWithValue("crack_facade_front_type", entity.CrackFacadeFrontType);
        context.AddParameterWithValue("crack_facade_front_size", entity.CrackFacadeFrontSize);
        context.AddParameterWithValue("crack_facade_back_restored", entity.CrackFacadeBackRestored);
        context.AddParameterWithValue("crack_facade_back_type", entity.CrackFacadeBackType);
        context.AddParameterWithValue("crack_facade_back_size", entity.CrackFacadeBackSize);
        context.AddParameterWithValue("crack_facade_left_restored", entity.CrackFacadeLeftRestored);
        context.AddParameterWithValue("crack_facade_left_type", entity.CrackFacadeLeftType);
        context.AddParameterWithValue("crack_facade_left_size", entity.CrackFacadeLeftSize);
        context.AddParameterWithValue("crack_facade_right_restored", entity.CrackFacadeRightRestored);
        context.AddParameterWithValue("crack_facade_right_type", entity.CrackFacadeRightType);
        context.AddParameterWithValue("crack_facade_right_size", entity.CrackFacadeRightSize);
        context.AddParameterWithValue("deformed_facade", entity.DeformedFacade);
        context.AddParameterWithValue("threshold_updown_skewed", entity.ThresholdUpdownSkewed);
        context.AddParameterWithValue("threshold_front_level", entity.ThresholdFrontLevel);
        context.AddParameterWithValue("threshold_back_level", entity.ThresholdBackLevel);
        context.AddParameterWithValue("skewed_parallel", entity.SkewedParallel);
        context.AddParameterWithValue("skewed_parallel_facade", entity.SkewedParallelFacade);
        context.AddParameterWithValue("skewed_perpendicular", entity.SkewedPerpendicular);
        context.AddParameterWithValue("skewed_perpendicular_facade", entity.SkewedPerpendicularFacade);
        context.AddParameterWithValue("settlement_speed", entity.SettlementSpeed);
        context.AddParameterWithValue("skewed_window_frame", entity.SkewedWindowFrame);
        context.AddParameterWithValue("facade_scan_risk", entity.FacadeScanRisk);
    }

    // private static InquirySample MapFromReader(DbDataReader reader, int offset = 0)
    //     => new()
    //     {
    //         Id = reader.GetInt(offset++),
    //         Inquiry = reader.GetInt(offset++),
    //         Address = reader.GetString(offset++),
    //         Building = reader.GetString(offset++),
    //         Note = reader.GetSafeString(offset++),
    //         CreateDate = reader.GetDateTime(offset++),
    //         UpdateDate = reader.GetSafeDateTime(offset++),
    //         DeleteDate = reader.GetSafeDateTime(offset++),
    //         BuiltYear = reader.GetSafeDateTime(offset++),
    //         Substructure = reader.GetFieldValue<Substructure?>(offset++),
    //         OverallQuality = reader.GetFieldValue<FoundationQuality?>(offset++),
    //         WoodQuality = reader.GetFieldValue<WoodQuality?>(offset++),
    //         ConstructionQuality = reader.GetFieldValue<Quality?>(offset++),
    //         WoodCapacityHorizontalQuality = reader.GetFieldValue<Quality?>(offset++),
    //         PileWoodCapacityVerticalQuality = reader.GetFieldValue<Quality?>(offset++),
    //         CarryingCapacityQuality = reader.GetFieldValue<Quality?>(offset++),
    //         MasonQuality = reader.GetFieldValue<Quality?>(offset++),
    //         WoodQualityNecessity = reader.GetSafeBoolean(offset++),
    //         ConstructionLevel = reader.GetSafeDecimal(offset++),
    //         WoodLevel = reader.GetSafeDecimal(offset++),
    //         PileDiameterTop = reader.GetSafeDecimal(offset++),
    //         PileDiameterBottom = reader.GetSafeDecimal(offset++),
    //         PileHeadLevel = reader.GetSafeDecimal(offset++),
    //         PileTipLevel = reader.GetSafeDecimal(offset++),
    //         FoundationDepth = reader.GetSafeDecimal(offset++),
    //         MasonLevel = reader.GetSafeDecimal(offset++),
    //         ConcreteChargerLength = reader.GetSafeDecimal(offset++),
    //         PileDistanceLength = reader.GetSafeDecimal(offset++),
    //         WoodPenetrationDepth = reader.GetSafeDecimal(offset++),
    //         Cpt = reader.GetSafeString(offset++),
    //         MonitoringWell = reader.GetSafeString(offset++),
    //         GroundwaterLevelTemp = reader.GetSafeDecimal(offset++),
    //         GroundLevel = reader.GetSafeDecimal(offset++),
    //         GroundwaterLevelNet = reader.GetSafeDecimal(offset++),
    //         FoundationType = reader.GetFieldValue<FoundationType?>(offset++),
    //         EnforcementTerm = reader.GetFieldValue<EnforcementTerm?>(offset++),
    //         RecoveryAdvised = reader.GetSafeBoolean(offset++),
    //         DamageCause = reader.GetFieldValue<FoundationDamageCause?>(offset++),
    //         DamageCharacteristics = reader.GetFieldValue<FoundationDamageCharacteristics?>(offset++),
    //         ConstructionPile = reader.GetFieldValue<ConstructionPile?>(offset++),
    //         WoodType = reader.GetFieldValue<WoodType?>(offset++),
    //         WoodEncroachement = reader.GetFieldValue<WoodEncroachement?>(offset++),
    //         CrackIndoorRestored = reader.GetSafeBoolean(offset++),
    //         CrackIndoorType = reader.GetFieldValue<CrackType?>(offset++),
    //         CrackIndoorSize = reader.GetSafeInt(offset++),
    //         CrackFacadeFrontRestored = reader.GetSafeBoolean(offset++),
    //         CrackFacadeFrontType = reader.GetFieldValue<CrackType?>(offset++),
    //         CrackFacadeFrontSize = reader.GetSafeInt(offset++),
    //         CrackFacadeBackRestored = reader.GetSafeBoolean(offset++),
    //         CrackFacadeBackType = reader.GetFieldValue<CrackType?>(offset++),
    //         CrackFacadeBackSize = reader.GetSafeInt(offset++),
    //         CrackFacadeLeftRestored = reader.GetSafeBoolean(offset++),
    //         CrackFacadeLeftType = reader.GetFieldValue<CrackType?>(offset++),
    //         CrackFacadeLeftSize = reader.GetSafeInt(offset++),
    //         CrackFacadeRightRestored = reader.GetSafeBoolean(offset++),
    //         CrackFacadeRightType = reader.GetFieldValue<CrackType?>(offset++),
    //         CrackFacadeRightSize = reader.GetSafeInt(offset++),
    //         DeformedFacade = reader.GetSafeBoolean(offset++),
    //         ThresholdUpdownSkewed = reader.GetSafeBoolean(offset++),
    //         ThresholdFrontLevel = reader.GetSafeDecimal(offset++),
    //         ThresholdBackLevel = reader.GetSafeDecimal(offset++),
    //         SkewedParallel = reader.GetSafeDecimal(offset++),
    //         SkewedParallelFacade = reader.GetFieldValue<RotationType?>(offset++),
    //         SkewedPerpendicular = reader.GetSafeDecimal(offset++),
    //         SkewedPerpendicularFacade = reader.GetFieldValue<RotationType?>(offset++),
    //         SettlementSpeed = reader.GetSafeDouble(offset++),
    //         SkewedWindowFrame = reader.GetSafeBoolean(offset++),
    //         FacadeScanRisk = reader.GetFieldValue<FacadeScanRisk?>(offset++),
    //     };

    public override Task<InquirySample> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Retrieve <see cref="InquirySample"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="InquirySample"/>.</returns>
    public async Task<InquirySample> GetByIdAsync(int id, Guid tenantId)
    {
        if (TryGetEntity(id, out InquirySample? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- InquirySample
                    s.id,
                    s.inquiry,
                    s.address,
                    b.external_id,
                    s.note,
                    s.create_date,
                    s.update_date,
                    s.delete_date,
                    s.built_year,
                    s.substructure,

                    -- Foundation Assessment
                    s.overall_quality,
                    s.wood_quality,
                    s.construction_quality,
                    s.wood_capacity_horizontal_quality,
                    s.pile_wood_capacity_vertical_quality,
                    s.carrying_capacity_quality,
                    s.mason_quality,
                    s.wood_quality_necessity,

                    -- Foundation Measurement
                    s.construction_level,
                    s.wood_level,
                    s.pile_diameter_top,
                    s.pile_diameter_bottom,
                    s.pile_head_level,
                    s.pile_tip_level,
                    s.foundation_depth,
                    s.mason_level,
                    s.concrete_charger_length,
                    s.pile_distance_length,
                    s.wood_penetration_depth,

                    -- Surrounding
                    s.cpt,
                    s.monitoring_well,
                    s.groundwater_level_temp,
                    s.groundlevel,
                    s.groundwater_level_net,
                    
                    -- Foundation
                    s.foundation_type,
                    s.enforcement_term,
                    s.recovery_advised,
                    s.damage_cause,
                    s.damage_characteristics,
                    s.construction_pile,
                    s.wood_type,
                    s.wood_encroachement,

                    -- Building
                    s.crack_indoor_restored,
                    s.crack_indoor_type,
                    s.crack_indoor_size,
                    s.crack_facade_front_restored,
                    s.crack_facade_front_type,
                    s.crack_facade_front_size,
                    s.crack_facade_back_restored,
                    s.crack_facade_back_type,
                    s.crack_facade_back_size,
                    s.crack_facade_left_restored,
                    s.crack_facade_left_type,
                    s.crack_facade_left_size,
                    s.crack_facade_right_restored,
                    s.crack_facade_right_type,
                    s.crack_facade_right_size,
                    s.deformed_facade,
                    s.threshold_updown_skewed,
                    s.threshold_front_level,
                    s.threshold_back_level,
                    s.skewed_parallel,
                    s.skewed_parallel_facade,
                    s.skewed_perpendicular,
                    s.skewed_perpendicular_facade,
                    s.settlement_speed,
                    s.skewed_window_frame,
                    s.facade_scan_risk
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            JOIN    geocoder.building b ON b.id = s.building
            WHERE   s.id = @id
            AND     a.owner = @tenant
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var inquiry_sample = await connection.QuerySingleOrDefaultAsync<InquirySample>(sql, new { id, tenant = tenantId });
        return inquiry_sample is null ? throw new EntityNotFoundException(nameof(InquirySample)) : CacheEntity(inquiry_sample);
    }

    public async IAsyncEnumerable<InquirySample> ListAllByBuildingIdAsync(string id)
    {
        var sql = @"
            SELECT  -- InquirySample
                    s.id,
                    s.inquiry,
                    s.address,
                    b.external_id,
                    s.note,
                    s.create_date,
                    s.update_date,
                    s.delete_date,
                    s.built_year,
                    s.substructure,

                    -- Foundation Assessment
                    s.overall_quality,
                    s.wood_quality,
                    s.construction_quality,
                    s.wood_capacity_horizontal_quality,
                    s.pile_wood_capacity_vertical_quality,
                    s.carrying_capacity_quality,
                    s.mason_quality,
                    s.wood_quality_necessity,

                    -- Foundation Measurement
                    s.construction_level,
                    s.wood_level,
                    s.pile_diameter_top,
                    s.pile_diameter_bottom,
                    s.pile_head_level,
                    s.pile_tip_level,
                    s.foundation_depth,
                    s.mason_level,
                    s.concrete_charger_length,
                    s.pile_distance_length,
                    s.wood_penetration_depth,

                    -- Surrounding
                    s.cpt,
                    s.monitoring_well,
                    s.groundwater_level_temp,
                    s.groundlevel,
                    s.groundwater_level_net,
                    
                    -- Foundation
                    s.foundation_type,
                    s.enforcement_term,
                    s.recovery_advised,
                    s.damage_cause,
                    s.damage_characteristics,
                    s.construction_pile,
                    s.wood_type,
                    s.wood_encroachement,

                    -- Building
                    s.crack_indoor_restored,
                    s.crack_indoor_type,
                    s.crack_indoor_size,
                    s.crack_facade_front_restored,
                    s.crack_facade_front_type,
                    s.crack_facade_front_size,
                    s.crack_facade_back_restored,
                    s.crack_facade_back_type,
                    s.crack_facade_back_size,
                    s.crack_facade_left_restored,
                    s.crack_facade_left_type,
                    s.crack_facade_left_size,
                    s.crack_facade_right_restored,
                    s.crack_facade_right_type,
                    s.crack_facade_right_size,
                    s.deformed_facade,
                    s.threshold_updown_skewed,
                    s.threshold_front_level,
                    s.threshold_back_level,
                    s.skewed_parallel,
                    s.skewed_parallel_facade,
                    s.skewed_perpendicular,
                    s.skewed_perpendicular_facade,
                    s.settlement_speed,
                    s.skewed_window_frame,
                    s.facade_scan_risk
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            JOIN    geocoder.building b ON b.id = s.building
            WHERE   s.building = @building
            ORDER BY s.create_date DESC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<InquirySample>(sql, new { building = id }))
        {
            yield return item;
        }
    }

    // TOOD: Remove
    /// <summary>
    ///     Retrieve all <see cref="InquirySample"/>.
    /// </summary>
    /// <returns>List of <see cref="InquirySample"/>.</returns>
    public async IAsyncEnumerable<InquirySample> ListAllAsync(Navigation navigation, Guid tenantId)
    {
        var sql = @"
            SELECT  -- InquirySample
                    s.id,
                    s.inquiry,
                    s.address,
                    b.external_id,
                    s.note,
                    s.create_date,
                    s.update_date,
                    s.delete_date,
                    s.built_year,
                    s.substructure,

                    -- Foundation Assessment
                    s.overall_quality,
                    s.wood_quality,
                    s.construction_quality,
                    s.wood_capacity_horizontal_quality,
                    s.pile_wood_capacity_vertical_quality,
                    s.carrying_capacity_quality,
                    s.mason_quality,
                    s.wood_quality_necessity,

                    -- Foundation Measurement
                    s.construction_level,
                    s.wood_level,
                    s.pile_diameter_top,
                    s.pile_diameter_bottom,
                    s.pile_head_level,
                    s.pile_tip_level,
                    s.foundation_depth,
                    s.mason_level,
                    s.concrete_charger_length,
                    s.pile_distance_length,
                    s.wood_penetration_depth,

                    -- Surrounding
                    s.cpt,
                    s.monitoring_well,
                    s.groundwater_level_temp,
                    s.groundlevel,
                    s.groundwater_level_net,
                    
                    -- Foundation
                    s.foundation_type,
                    s.enforcement_term,
                    s.recovery_advised,
                    s.damage_cause,
                    s.damage_characteristics,
                    s.construction_pile,
                    s.wood_type,
                    s.wood_encroachement,

                    -- Building
                    s.crack_indoor_restored,
                    s.crack_indoor_type,
                    s.crack_indoor_size,
                    s.crack_facade_front_restored,
                    s.crack_facade_front_type,
                    s.crack_facade_front_size,
                    s.crack_facade_back_restored,
                    s.crack_facade_back_type,
                    s.crack_facade_back_size,
                    s.crack_facade_left_restored,
                    s.crack_facade_left_type,
                    s.crack_facade_left_size,
                    s.crack_facade_right_restored,
                    s.crack_facade_right_type,
                    s.crack_facade_right_size,
                    s.deformed_facade,
                    s.threshold_updown_skewed,
                    s.threshold_front_level,
                    s.threshold_back_level,
                    s.skewed_parallel,
                    s.skewed_parallel_facade,
                    s.skewed_perpendicular,
                    s.skewed_perpendicular_facade,
                    s.settlement_speed,
                    s.skewed_window_frame,
                    s.facade_scan_risk
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            JOIN    geocoder.building b ON b.id = s.building
            WHERE   a.owner = @tenant
            ORDER BY s.create_date DESC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<InquirySample>(sql, new { tenant = tenantId }))
        {
            yield return item;
        }
    }

    /// <summary>
    ///     Retrieve all entities and filter on report.
    /// </summary>
    /// <returns>List of entities.</returns>
    public async IAsyncEnumerable<InquirySample> ListAllAsync(int report, Navigation navigation, Guid tenantId)
    {
        var sql = @"
            SELECT  -- InquirySample
                    s.id,
                    s.inquiry,
                    s.address,
                    b.external_id,
                    s.note,
                    s.create_date,
                    s.update_date,
                    s.delete_date,
                    s.built_year,
                    s.substructure,

                    -- Foundation Assessment
                    s.overall_quality,
                    s.wood_quality,
                    s.construction_quality,
                    s.wood_capacity_horizontal_quality,
                    s.pile_wood_capacity_vertical_quality,
                    s.carrying_capacity_quality,
                    s.mason_quality,
                    s.wood_quality_necessity,

                    -- Foundation Measurement
                    s.construction_level,
                    s.wood_level,
                    s.pile_diameter_top,
                    s.pile_diameter_bottom,
                    s.pile_head_level,
                    s.pile_tip_level,
                    s.foundation_depth,
                    s.mason_level,
                    s.concrete_charger_length,
                    s.pile_distance_length,
                    s.wood_penetration_depth,

                    -- Surrounding
                    s.cpt,
                    s.monitoring_well,
                    s.groundwater_level_temp,
                    s.groundlevel,
                    s.groundwater_level_net,
                    
                    -- Foundation
                    s.foundation_type,
                    s.enforcement_term,
                    s.recovery_advised,
                    s.damage_cause,
                    s.damage_characteristics,
                    s.construction_pile,
                    s.wood_type,
                    s.wood_encroachement,

                    -- Building
                    s.crack_indoor_restored,
                    s.crack_indoor_type,
                    s.crack_indoor_size,
                    s.crack_facade_front_restored,
                    s.crack_facade_front_type,
                    s.crack_facade_front_size,
                    s.crack_facade_back_restored,
                    s.crack_facade_back_type,
                    s.crack_facade_back_size,
                    s.crack_facade_left_restored,
                    s.crack_facade_left_type,
                    s.crack_facade_left_size,
                    s.crack_facade_right_restored,
                    s.crack_facade_right_type,
                    s.crack_facade_right_size,
                    s.deformed_facade,
                    s.threshold_updown_skewed,
                    s.threshold_front_level,
                    s.threshold_back_level,
                    s.skewed_parallel,
                    s.skewed_parallel_facade,
                    s.skewed_perpendicular,
                    s.skewed_perpendicular_facade,
                    s.settlement_speed,
                    s.skewed_window_frame,
                    s.facade_scan_risk
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            JOIN    geocoder.building b ON b.id = s.building
            WHERE   a.owner = @tenant
            AND     i.id = @id
            ORDER BY s.create_date DESC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<InquirySample>(sql, new { id = report, tenant = tenantId }))
        {
            yield return item;
        }
    }

    public async Task UpdateAsync(InquirySample entity, Guid tenantId)
    {
        ResetCacheEntity(entity);

        var sql = @"
            UPDATE  report.inquiry_sample AS s
            SET     -- InquirySample
                    inquiry = @inquiry,
                    address = @address,
                    building = @building,
                    note = NULLIF(trim(@note), ''),
                    built_year = @built_year,
                    substructure = @substructure,

                    -- Foundation Assessment
                    overall_quality = @overall_quality,
                    wood_quality = @wood_quality,
                    construction_quality = @construction_quality,
                    wood_capacity_horizontal_quality = @wood_capacity_horizontal_quality,
                    pile_wood_capacity_vertical_quality = @pile_wood_capacity_vertical_quality,
                    carrying_capacity_quality = @carrying_capacity_quality,
                    mason_quality = @mason_quality,
                    wood_quality_necessity = @wood_quality_necessity,

                    -- Foundation Measurement
                    construction_level = @construction_level,
                    wood_level = @wood_level,
                    pile_diameter_top = @pile_diameter_top,
                    pile_diameter_bottom = @pile_diameter_bottom,
                    pile_head_level = @pile_head_level,
                    pile_tip_level = @pile_tip_level,
                    foundation_depth = @foundation_depth,
                    mason_level = @mason_level,
                    concrete_charger_length = @concrete_charger_length,
                    pile_distance_length = @pile_distance_length,
                    wood_penetration_depth = @wood_penetration_depth,

                    -- Surrounding
                    cpt = NULLIF(trim(@cpt), ''),
                    monitoring_well = NULLIF(trim(@monitoring_well), ''),
                    groundwater_level_temp = @groundwater_level_temp,
                    groundlevel = @groundlevel,
                    groundwater_level_net = @groundwater_level_net,
                
                    -- Foundation
                    foundation_type = @foundation_type,
                    enforcement_term = @enforcement_term,
                    recovery_advised = @recovery_advised,
                    damage_cause = @damage_cause,
                    damage_characteristics = @damage_characteristics,
                    construction_pile = @construction_pile,
                    wood_type = @wood_type,
                    wood_encroachement = @wood_encroachement,

                    -- Building
                    crack_indoor_restored = @crack_indoor_restored,
                    crack_indoor_type = @crack_indoor_type,
                    crack_indoor_size = @crack_indoor_size,
                    crack_facade_front_restored = @crack_facade_front_restored,
                    crack_facade_front_type = @crack_facade_front_type,
                    crack_facade_front_size = @crack_facade_front_size,
                    crack_facade_back_restored = @crack_facade_back_restored,
                    crack_facade_back_type = @crack_facade_back_type,
                    crack_facade_back_size = @crack_facade_back_size,
                    crack_facade_left_restored = @crack_facade_left_restored,
                    crack_facade_left_type = @crack_facade_left_type,
                    crack_facade_left_size = @crack_facade_left_size,
                    crack_facade_right_restored = @crack_facade_right_restored,
                    crack_facade_right_type = @crack_facade_right_type,
                    crack_facade_right_size = @crack_facade_right_size,
                    deformed_facade = @deformed_facade,
                    threshold_updown_skewed = @threshold_updown_skewed,
                    threshold_front_level = @threshold_front_level,
                    threshold_back_level = @threshold_back_level,
                    skewed_parallel = @skewed_parallel,
                    skewed_parallel_facade = @skewed_parallel_facade,
                    skewed_perpendicular = @skewed_perpendicular,
                    skewed_perpendicular_facade = @skewed_perpendicular_facade,
                    settlement_speed = @settlement_speed,
                    skewed_window_frame = @skewed_window_frame,

                    facade_scan_risk = @facade_scan_risk
            FROM 	application.attribution AS a, report.inquiry AS i
            WHERE   i.id = s.inquiry
            AND     a.id = i.attribution
            AND     s.id = @id
            AND     a.owner = @tenant";

        // TOOD: Dapper ORM

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", entity.Id);
        context.AddParameterWithValue("tenant", tenantId);

        MapToWriter(context, entity);

        await context.NonQueryAsync();
    }
}
