using FunderMaps.Core;
using FunderMaps.Core.Entities;
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
                skewed_window_frame)
            VALUES (
                @inquiry,
                @address,
                (
                    SELECT       a.building_id 
	                FROM         geocoder.address a 
	                WHERE a.id = @address
                ),
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
                @skewed_window_frame)
            RETURNING id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        MapToWriter(context, entity);

        return await context.ScalarAsync<int>();
    }

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            WHERE   a.owner = @tenant";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("tenant", AppContext.TenantId);

        return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public async Task<long> CountAsync(int report)
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.inquiry_sample AS s
            JOIN    report.inquiry AS i ON i.id = s.inquiry
            JOIN    application.attribution AS a ON a.id = i.attribution
            WHERE   a.owner = @tenant
            AND     i.id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", report);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Delete <see cref="InquirySample"/>.
    /// </summary>
    /// <param name="id">Entity object.</param>
    public override async Task DeleteAsync(int id)
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await context.NonQueryAsync();
    }

    private static void MapToWriter(DbContext context, InquirySample entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        context.AddParameterWithValue("inquiry", entity.Inquiry);
        context.AddParameterWithValue("address", entity.Address);
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
    }

    private static InquirySample MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Id = reader.GetInt(offset + 0),
            Inquiry = reader.GetInt(offset + 1),
            Address = reader.GetString(offset + 2),
            Note = reader.GetSafeString(offset + 3),
            CreateDate = reader.GetDateTime(offset + 4),
            UpdateDate = reader.GetSafeDateTime(offset + 5),
            DeleteDate = reader.GetSafeDateTime(offset + 6),
            BuiltYear = reader.GetSafeDateTime(offset + 7),
            Substructure = reader.GetFieldValue<Substructure?>(offset + 8),
            OverallQuality = reader.GetFieldValue<FoundationQuality?>(offset + 9),
            WoodQuality = reader.GetFieldValue<WoodQuality?>(offset + 10),
            ConstructionQuality = reader.GetFieldValue<Quality?>(offset + 11),
            WoodCapacityHorizontalQuality = reader.GetFieldValue<Quality?>(offset + 12),
            PileWoodCapacityVerticalQuality = reader.GetFieldValue<Quality?>(offset + 13),
            CarryingCapacityQuality = reader.GetFieldValue<Quality?>(offset + 14),
            MasonQuality = reader.GetFieldValue<Quality?>(offset + 15),
            WoodQualityNecessity = reader.GetSafeBoolean(offset + 16),
            ConstructionLevel = reader.GetSafeDecimal(offset + 17),
            WoodLevel = reader.GetSafeDecimal(offset + 18),
            PileDiameterTop = reader.GetSafeDecimal(offset + 19),
            PileDiameterBottom = reader.GetSafeDecimal(offset + 20),
            PileHeadLevel = reader.GetSafeDecimal(offset + 21),
            PileTipLevel = reader.GetSafeDecimal(offset + 22),
            FoundationDepth = reader.GetSafeDecimal(offset + 23),
            MasonLevel = reader.GetSafeDecimal(offset + 24),
            ConcreteChargerLength = reader.GetSafeDecimal(offset + 25),
            PileDistanceLength = reader.GetSafeDecimal(offset + 26),
            WoodPenetrationDepth = reader.GetSafeDecimal(offset + 27),
            Cpt = reader.GetSafeString(offset + 28),
            MonitoringWell = reader.GetSafeString(offset + 29),
            GroundwaterLevelTemp = reader.GetSafeDecimal(offset + 30),
            GroundLevel = reader.GetSafeDecimal(offset + 31),
            GroundwaterLevelNet = reader.GetSafeDecimal(offset + 32),
            FoundationType = reader.GetFieldValue<FoundationType?>(offset + 33),
            EnforcementTerm = reader.GetFieldValue<EnforcementTerm?>(offset + 34),
            RecoveryAdvised = reader.GetSafeBoolean(offset + 35),
            DamageCause = reader.GetFieldValue<FoundationDamageCause?>(offset + 36),
            DamageCharacteristics = reader.GetFieldValue<FoundationDamageCharacteristics?>(offset + 37),
            ConstructionPile = reader.GetFieldValue<ConstructionPile?>(offset + 38),
            WoodType = reader.GetFieldValue<WoodType?>(offset + 39),
            WoodEncroachement = reader.GetFieldValue<WoodEncroachement?>(offset + 40),
            CrackIndoorRestored = reader.GetSafeBoolean(offset + 41),
            CrackIndoorType = reader.GetFieldValue<CrackType?>(offset + 42),
            CrackIndoorSize = reader.GetSafeDecimal(offset + 43),
            CrackFacadeFrontRestored = reader.GetSafeBoolean(offset + 44),
            CrackFacadeFrontType = reader.GetFieldValue<CrackType?>(offset + 45),
            CrackFacadeFrontSize = reader.GetSafeDecimal(offset + 46),
            CrackFacadeBackRestored = reader.GetSafeBoolean(offset + 47),
            CrackFacadeBackType = reader.GetFieldValue<CrackType?>(offset + 48),
            CrackFacadeBackSize = reader.GetSafeDecimal(offset + 49),
            CrackFacadeLeftRestored = reader.GetSafeBoolean(offset + 50),
            CrackFacadeLeftType = reader.GetFieldValue<CrackType?>(offset + 51),
            CrackFacadeLeftSize = reader.GetSafeDecimal(offset + 52),
            CrackFacadeRightRestored = reader.GetSafeBoolean(offset + 53),
            CrackFacadeRightType = reader.GetFieldValue<CrackType?>(offset + 54),
            CrackFacadeRightSize = reader.GetSafeDecimal(offset + 55),
            DeformedFacade = reader.GetSafeBoolean(offset + 56),
            ThresholdUpdownSkewed = reader.GetSafeBoolean(offset + 57),
            ThresholdFrontLevel = reader.GetSafeDecimal(offset + 58),
            ThresholdBackLevel = reader.GetSafeDecimal(offset + 59),
            SkewedParallel = reader.GetSafeDecimal(offset + 60),
            SkewedParallelFacade = reader.GetFieldValue<RotationType?>(offset + 61),
            SkewedPerpendicular = reader.GetSafeDecimal(offset + 62),
            SkewedPerpendicularFacade = reader.GetFieldValue<RotationType?>(offset + 63),
            SettlementSpeed = reader.GetSafeDouble(offset + 64),
            SkewedWindowFrame = reader.GetSafeBoolean(offset + 65),
        };

    /// <summary>
    ///     Retrieve <see cref="InquirySample"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="InquirySample"/>.</returns>
    public override async Task<InquirySample> GetByIdAsync(int id)
    {
        if (TryGetEntity(id, out InquirySample entity))
        {
            return entity;
        }

        var sql = @"
            SELECT  -- InquirySample
                    s.id,
                    s.inquiry,
                    s.address,
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
                    s.skewed_window_frame
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            WHERE   s.id = @id
            AND     a.owner = @tenant
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await using var reader = await context.ReaderAsync();

        return CacheEntity(MapFromReader(reader));
    }

    /// <summary>
    ///     Retrieve all <see cref="InquirySample"/>.
    /// </summary>
    /// <returns>List of <see cref="InquirySample"/>.</returns>
    public override async IAsyncEnumerable<InquirySample> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- InquirySample
                    s.id,
                    s.inquiry,
                    s.address,
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
                    s.skewed_window_frame
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            WHERE   a.owner = @tenant
            ORDER BY s.create_date DESC";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }

    /// <summary>
    ///     Retrieve all entities and filter on report.
    /// </summary>
    /// <returns>List of entities.</returns>
    public async IAsyncEnumerable<InquirySample> ListAllAsync(int report, Navigation navigation)
    {
        var sql = @"
            SELECT  -- InquirySample
                    s.id,
                    s.inquiry,
                    s.address,
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
                    s.skewed_window_frame
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            WHERE   a.owner = @tenant
            AND     i.id = @id
            ORDER BY s.create_date DESC";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", report);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }

    public override async Task UpdateAsync(InquirySample entity)
    {
        ResetCacheEntity(entity);

        var sql = @"
            UPDATE  report.inquiry_sample AS s
            SET     -- InquirySample
                    inquiry = @inquiry,
                    address = @address,
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
                    skewed_window_frame = @skewed_window_frame
            FROM 	application.attribution AS a, report.inquiry AS i
            WHERE   i.id = s.inquiry
            AND     a.id = i.attribution
            AND     s.id = @id
            AND     a.owner = @tenant";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", entity.Id);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        MapToWriter(context, entity);

        await context.NonQueryAsync();
    }
}
