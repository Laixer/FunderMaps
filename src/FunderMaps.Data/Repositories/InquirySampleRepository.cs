using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
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
        public override async ValueTask<int> AddAsync(InquirySample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO report.inquiry_sample(
                    inquiry,
                    address,
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
                    recovery_adviced,
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
                    skewed_perpendicular,
                    skewed_facade,
                    settlement_speed)
                VALUES (
                    @inquiry,
                    @address,
                    @note,
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
                    @cpt,
                    @monitoring_well,
                    @groundwater_level_temp,
                    @groundlevel,
                    @groundwater_level_net,
                    @foundation_type,
                    @enforcement_term,
                    @recovery_adviced,
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
                    @skewed_perpendicular,
                    @skewed_facade,
                    @settlement_speed)
                RETURNING id";

            await using var context = await DbContextFactory(sql);

            MapToWriter(context, entity);

            return await context.ScalarAsync<int>();
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async ValueTask<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.inquiry_sample AS s
                JOIN 	report.inquiry AS i ON i.id = s.inquiry
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   a.owner = @tenant";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("tenant", AppContext.TenantId);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="InquirySample"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.inquiry_sample AS s
                USING 	application.attribution AS a, report.inquiry AS i
                WHERE   i.id = s.inquiry
                AND     a.id = i.attribution
                AND     s.id = @id
                AND     a.owner = @tenant";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await context.NonQueryAsync();
        }

        private static void MapToWriter(DbContext context, InquirySample entity)
        {
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
            context.AddParameterWithValue("recovery_adviced", entity.RecoveryAdvised);
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
            context.AddParameterWithValue("skewed_perpendicular", entity.SkewedPerpendicular);
            context.AddParameterWithValue("skewed_facade", entity.SkewedFacade);
            context.AddParameterWithValue("settlement_speed", entity.SettlementSpeed);
        }

        private static InquirySample MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new InquirySample
            {
                Id = reader.GetInt(offset + 0),
                Inquiry = reader.GetInt(offset + 1),
                Address = reader.GetSafeString(offset + 2),
                Note = reader.GetSafeString(offset + 3),
                CreateDate = reader.GetDateTime(offset + 4),
                UpdateDate = reader.GetSafeDateTime(offset + 5),
                DeleteDate = reader.GetSafeDateTime(offset + 6),
                BaseMeasurementLevel = reader.GetFieldValue<BaseMeasurementLevel>(offset + 7),
                BuiltYear = reader.GetDateTime(offset + 8),
                Substructure = reader.GetFieldValue<Substructure>(offset + 9),
                OverallQuality = reader.GetFieldValue<FoundationQuality?>(offset + 10),
                WoodQuality = reader.GetFieldValue<WoodQuality?>(offset + 11),
                ConstructionQuality = reader.GetFieldValue<Quality?>(offset + 12),
                WoodCapacityHorizontalQuality = reader.GetFieldValue<Quality?>(offset + 13),
                PileWoodCapacityVerticalQuality = reader.GetFieldValue<Quality?>(offset + 14),
                CarryingCapacityQuality = reader.GetFieldValue<Quality?>(offset + 15),
                MasonQuality = reader.GetFieldValue<Quality?>(offset + 16),
                WoodQualityNecessity = reader.GetSafeBoolean(offset + 17),
                ConstructionLevel = reader.GetSafeDecimal(offset + 18),
                WoodLevel = reader.GetSafeDecimal(offset + 19),
                PileDiameterTop = reader.GetSafeDecimal(offset + 20),
                PileDiameterBottom = reader.GetSafeDecimal(offset + 21),
                PileHeadLevel = reader.GetSafeDecimal(offset + 22),
                PileTipLevel = reader.GetSafeDecimal(offset + 23),
                FoundationDepth = reader.GetSafeDecimal(offset + 24),
                MasonLevel = reader.GetSafeDecimal(offset + 25),
                ConcreteChargerLength = reader.GetSafeDecimal(offset + 26),
                PileDistanceLength = reader.GetSafeDecimal(offset + 27),
                WoodPenetrationDepth = reader.GetSafeDecimal(offset + 28),
                Cpt = reader.GetSafeString(offset + 29),
                MonitoringWell = reader.GetSafeString(offset + 30),
                GroundwaterLevelTemp = reader.GetSafeDecimal(offset + 31),
                GroundLevel = reader.GetSafeDecimal(offset + 32),
                GroundwaterLevelNet = reader.GetSafeDecimal(offset + 33),
                FoundationType = reader.GetFieldValue<FoundationType?>(offset + 34),
                EnforcementTerm = reader.GetFieldValue<EnforcementTerm?>(offset + 35),
                RecoveryAdvised = reader.GetSafeBoolean(offset + 36),
                DamageCause = reader.GetFieldValue<FoundationDamageCause>(offset + 37),
                DamageCharacteristics = reader.GetFieldValue<FoundationDamageCharacteristics?>(offset + 38),
                ConstructionPile = reader.GetFieldValue<ConstructionPile?>(offset + 39),
                WoodType = reader.GetFieldValue<WoodType?>(offset + 40),
                WoodEncroachement = reader.GetFieldValue<WoodEncroachement?>(offset + 41),
                CrackIndoorRestored = reader.GetSafeBoolean(offset + 42),
                CrackIndoorType = reader.GetFieldValue<CrackType?>(offset + 43),
                CrackIndoorSize = reader.GetSafeDecimal(offset + 44),
                CrackFacadeFrontRestored = reader.GetSafeBoolean(offset + 45),
                CrackFacadeFrontType = reader.GetFieldValue<CrackType?>(offset + 46),
                CrackFacadeFrontSize = reader.GetSafeDecimal(offset + 47),
                CrackFacadeBackRestored = reader.GetSafeBoolean(offset + 48),
                CrackFacadeBackType = reader.GetFieldValue<CrackType?>(offset + 49),
                CrackFacadeBackSize = reader.GetSafeDecimal(offset + 50),
                CrackFacadeLeftRestored = reader.GetSafeBoolean(offset + 51),
                CrackFacadeLeftType = reader.GetFieldValue<CrackType?>(offset + 52),
                CrackFacadeLeftSize = reader.GetSafeDecimal(offset + 53),
                CrackFacadeRightRestored = reader.GetSafeBoolean(offset + 54),
                CrackFacadeRightType = reader.GetFieldValue<CrackType?>(offset + 55),
                CrackFacadeRightSize = reader.GetSafeDecimal(offset + 56),
                DeformedFacade = reader.GetSafeBoolean(offset + 57),
                ThresholdUpdownSkewed = reader.GetSafeBoolean(offset + 58),
                ThresholdFrontLevel = reader.GetSafeDecimal(offset + 59),
                ThresholdBackLevel = reader.GetSafeDecimal(offset + 60),
                SkewedParallel = reader.GetSafeDecimal(offset + 61),
                SkewedPerpendicular = reader.GetSafeDecimal(offset + 62),
                SkewedFacade = reader.GetFieldValue<RotationType?>(offset + 63),
                SettlementSpeed = reader.GetSafeDouble(offset + 64),
            };

        /// <summary>
        ///     Retrieve <see cref="InquirySample"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="InquirySample"/>.</returns>
        public override async ValueTask<InquirySample> GetByIdAsync(int id)
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
                        s.base_measurement_level,
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
                        s.recovery_adviced,
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
                        s.skewed_perpendicular,
                        s.skewed_facade,
                        s.settlement_speed
                FROM    report.inquiry_sample AS s
                JOIN 	report.inquiry AS i ON i.id = s.inquiry
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   s.id = @id
                AND     a.owner = @tenant
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        public Task<InquirySample> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Retrieve all <see cref="InquirySample"/>.
        /// </summary>
        /// <returns>List of <see cref="InquirySample"/>.</returns>
        public override async IAsyncEnumerable<InquirySample> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
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
                        s.base_measurement_level,
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
                        s.recovery_adviced,
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
                        s.skewed_perpendicular,
                        s.skewed_facade,
                        s.settlement_speed
                FROM    report.inquiry_sample AS s
                JOIN 	report.inquiry AS i ON i.id = s.inquiry
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   a.owner = @tenant";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Retrieve all entities and filter on report.
        /// </summary>
        /// <returns>List of entities.</returns>
        public async IAsyncEnumerable<InquirySample> ListAllAsync(int report, INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
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
                        s.base_measurement_level,
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
                        s.recovery_adviced,
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
                        s.skewed_perpendicular,
                        s.skewed_facade,
                        s.settlement_speed
                FROM    report.inquiry_sample AS s
                JOIN 	report.inquiry AS i ON i.id = s.inquiry
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   a.owner = @tenant
                AND     i.id = @id";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", report);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        public override async ValueTask UpdateAsync(InquirySample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  report.inquiry_sample AS s
                    SET     
                        -- InquirySample
                        inquiry = @inquiry,
                        address = @address,
                        note = @note,
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
                        cpt = @cpt,
                        monitoring_well = @monitoring_well,
                        groundwater_level_temp = @groundwater_level_temp,
                        groundlevel = @groundlevel,
                        groundwater_level_net = @groundwater_level_net,
                        
                        -- Foundation
                        foundation_type = @foundation_type,
                        enforcement_term = @enforcement_term,
                        recovery_adviced = @recovery_adviced,
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
                        skewed_perpendicular = @skewed_perpendicular,
                        skewed_facade = @skewed_facade,
                        settlement_speed = @settlement_speed


                    FROM 	application.attribution AS a, report.inquiry AS i
                    WHERE   i.id = s.inquiry
                    AND     a.id = i.attribution
                    AND     s.id = @id
                    AND     a.owner = @tenant";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}
