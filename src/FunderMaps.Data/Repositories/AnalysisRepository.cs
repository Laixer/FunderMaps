using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Repository for analysis products.
    /// </summary>
    internal sealed class AnalysisRepository : DbServiceBase, IAnalysisRepository
    {
        /// <summary>
        ///     Gets an analysis product by its internal building id.
        /// </summary>
        /// <param name="id">Internal building id.</param>
        public async Task<AnalysisProduct> GetByIdAsync(string id)
        {
            var sql = @"
                SELECT -- AnalysisAddress
                        aa.id,
                        aa.external_id,
                        aa.external_source,
                        aa.construction_year,
                        aa.construction_year_source,
                        aa.address_id,
                        aa.address_external_id,
                        aa.postal_code,
                        aa.neighborhood_id,
                        aa.groundwater_level,
                        aa.soil,
                        aa.building_height,
                        aa.ground_level,
                        aa.cpt,
                        aa.monitoring_well,
                        aa.recovery_advised,
                        aa.damage_cause,
                        aa.substructure,
                        aa.document_name,
                        aa.document_date,
                        aa.inquiry_type,
                        aa.recovery_type,
                        aa.recovery_status,
                        aa.surface_area,
                        aa.living_area,
                        aa.foundation_bearing_layer,
                        aa.restoration_costs,
                        aa.foundation_type,
                        aa.foundation_type_reliability,
                        aa.drystand,
                        aa.drystand_reliability,
                        aa.drystand_risk,
                        aa.dewatering_depth,
                        aa.dewatering_depth_reliability,
                        aa.dewatering_depth_risk,
                        aa.bio_infection,
                        aa.bio_infection_reliability,
                        aa.bio_infection_risk
                FROM    data.analysis_address AS aa
                WHERE   aa.id = @id
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its internal building id.
        /// </summary>
        /// <param name="id">Internal building id.</param>
        public async Task<AnalysisProduct2> GetById2Async(string id)
        {
            var sql = @"
                SELECT -- AnalysisComplete
                        ac.building_id,
                        ac.external_building_id,
                        ac.address_id,
                        ac.address_external_id,
                        ac.neighborhood_id,
                        ac.construction_year,
                        ac.foundation_type,
                        ac.foundation_type_reliability,
                        ac.restoration_costs,
                        ac.drystand_risk,
                        ac.drystand_risk_reliability,
                        ac.bio_infection_risk,
                        ac.bio_infection_risk_reliability,
                        ac.dewatering_depth_risk,
                        ac.dewatering_depth_risk_reliability,
                        ac.unclassified_risk,
                        ac.recovery_type
                FROM    data.analysis_complete ac
                WHERE   ac.building_id = @id
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader2(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its external building id and source.
        /// </summary>
        /// <param name="id">External building id.</param>
        public async Task<AnalysisProduct> GetByExternalIdAsync(string id)
        {
            var sql = @"
                SELECT -- AnalysisAddress
                        aa.id,
                        aa.external_id,
                        aa.external_source,
                        aa.construction_year,
                        aa.construction_year_source,
                        aa.address_id,
                        aa.address_external_id,
                        aa.postal_code,
                        aa.neighborhood_id,
                        aa.groundwater_level,
                        aa.soil,
                        aa.building_height,
                        aa.ground_level,
                        aa.cpt,
                        aa.monitoring_well,
                        aa.recovery_advised,
                        aa.damage_cause,
                        aa.substructure,
                        aa.document_name,
                        aa.document_date,
                        aa.inquiry_type,
                        aa.recovery_type,
                        aa.recovery_status,
                        aa.surface_area,
                        aa.living_area,
                        aa.foundation_bearing_layer,
                        aa.restoration_costs,
                        aa.foundation_type,
                        aa.foundation_type_reliability,
                        aa.drystand,
                        aa.drystand_reliability,
                        aa.drystand_risk,
                        aa.dewatering_depth,
                        aa.dewatering_depth_reliability,
                        aa.dewatering_depth_risk,
                        aa.bio_infection,
                        aa.bio_infection_reliability,
                        aa.bio_infection_risk
                FROM    data.analysis_address AS aa
                WHERE   aa.external_id = upper(@external_id)
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("external_id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its external building id and source.
        /// </summary>
        /// <param name="id">External building id.</param>
        public async Task<AnalysisProduct2> GetByExternalId2Async(string id)
        {
            var sql = @"
                SELECT -- AnalysisComplete
                        ac.building_id,
                        ac.external_building_id,
                        ac.address_id,
                        ac.address_external_id,
                        ac.neighborhood_id,
                        ac.construction_year,
                        ac.foundation_type,
                        ac.foundation_type_reliability,
                        ac.restoration_costs,
                        ac.drystand_risk,
                        ac.drystand_risk_reliability,
                        ac.bio_infection_risk,
                        ac.bio_infection_risk_reliability,
                        ac.dewatering_depth_risk,
                        ac.dewatering_depth_risk_reliability,
                        ac.unclassified_risk,
                        ac.recovery_type
                FROM    data.analysis_complete ac
                WHERE   ac.external_building_id = upper(@external_id)
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("external_id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader2(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its external address id and source.
        /// </summary>
        /// <param name="id">External address id.</param>
        public async Task<AnalysisProduct> GetByAddressExternalIdAsync(string id)
        {
            var sql = @"
                SELECT -- AnalysisAddress
                        aa.id,
                        aa.external_id,
                        aa.external_source,
                        aa.construction_year,
                        aa.construction_year_source,
                        aa.address_id,
                        aa.address_external_id,
                        aa.postal_code,
                        aa.neighborhood_id,
                        aa.groundwater_level,
                        aa.soil,
                        aa.building_height,
                        aa.ground_level,
                        aa.cpt,
                        aa.monitoring_well,
                        aa.recovery_advised,
                        aa.damage_cause,
                        aa.substructure,
                        aa.document_name,
                        aa.document_date,
                        aa.inquiry_type,
                        aa.recovery_type,
                        aa.recovery_status,
                        aa.surface_area,
                        aa.living_area,
                        aa.foundation_bearing_layer,
                        aa.restoration_costs,
                        aa.foundation_type,
                        aa.foundation_type_reliability,
                        aa.drystand,
                        aa.drystand_reliability,
                        aa.drystand_risk,
                        aa.dewatering_depth,
                        aa.dewatering_depth_reliability,
                        aa.dewatering_depth_risk,
                        aa.bio_infection,
                        aa.bio_infection_reliability,
                        aa.bio_infection_risk
                FROM    data.analysis_address AS aa
                WHERE   aa.address_external_id = upper(@external_id)
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("external_id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its external address id and source.
        /// </summary>
        /// <param name="id">External address id.</param>
        public async Task<AnalysisProduct2> GetByAddressExternalId2Async(string id)
        {
            var sql = @"
                SELECT -- AnalysisComplete
                        ac.building_id,
                        ac.external_building_id,
                        ac.address_id,
                        ac.address_external_id,
                        ac.neighborhood_id,
                        ac.construction_year,
                        ac.foundation_type,
                        ac.foundation_type_reliability,
                        ac.restoration_costs,
                        ac.drystand_risk,
                        ac.drystand_risk_reliability,
                        ac.bio_infection_risk,
                        ac.bio_infection_risk_reliability,
                        ac.dewatering_depth_risk,
                        ac.dewatering_depth_risk_reliability,
                        ac.unclassified_risk,
                        ac.recovery_type
                FROM    data.analysis_complete ac
                WHERE   ac.address_external_id = upper(@external_id)
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("external_id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader2(reader);
        }

        /// <summary>
        ///     Gets the risk index by its internal building id.
        /// </summary>
        /// <param name="id">Internal building id.</param>
        public async Task<bool> GetRiskIndexByIdAsync(string id)
        {
            var sql = @"
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
                FROM    data.analysis_complete ac
                WHERE   ac.building_id = @id
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            return await context.ScalarAsync<bool>();
        }

        /// <summary>
        ///     Gets the risk index by its external building id and source.
        /// </summary>
        /// <param name="id">Internal building id.</param>
        public async Task<bool> GetRiskIndexByExternalIdAsync(string id)
        {
            var sql = @"
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
                FROM    data.analysis_complete ac
                WHERE   ac.external_building_id = upper(@external_id)
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("external_id", id);

            return await context.ScalarAsync<bool>();
        }

        /// <summary>
        ///     Gets the risk index by its external address id and source.
        /// </summary>
        /// <param name="id">Internal building id.</param>
        public async Task<bool> GetRiskIndexByAddressExternalIdAsync(string id)
        {
            var sql = @"
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
                FROM    data.analysis_complete ac
                WHERE   ac.address_external_id = upper(@external_id)
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("external_id", id);

            return await context.ScalarAsync<bool>();
        }

        /// <summary>
        ///     Maps a reader to an <see cref="AnalysisProduct"/>.
        /// </summary>
        public static AnalysisProduct MapFromReader(DbDataReader reader, int offset = 0)
            => new()
            {
                Id = reader.GetSafeString(offset),
                ExternalId = reader.GetSafeString(offset + 1),
                ExternalSource = reader.GetFieldValue<ExternalDataSource>(offset + 2),
                ConstructionYear = reader.GetDateTime(offset + 3),
                ConstructionYearSource = reader.GetFieldValue<BuiltYearSource>(offset + 4),
                AddressId = reader.GetSafeString(offset + 5),
                AddressExternalId = reader.GetSafeString(offset + 6),
                PostalCode = reader.GetSafeString(offset + 7),
                NeighborhoodId = reader.GetSafeString(offset + 8),
                GroundWaterLevel = reader.GetSafeDouble(offset + 9),
                Soil = reader.GetSafeString(offset + 10),
                BuildingHeight = reader.GetSafeDouble(offset + 11),
                GroundLevel = reader.GetSafeDouble(offset + 12),
                Cpt = reader.GetSafeString(offset + 13),
                MonitoringWell = reader.GetSafeString(offset + 14),
                RecoveryAdvised = reader.GetSafeBoolean(offset + 15),
                DamageCause = reader.GetFieldValue<FoundationDamageCause?>(offset + 16),
                Substructure = reader.GetFieldValue<Substructure?>(offset + 17),
                DocumentName = reader.GetSafeString(offset + 18),
                DocumentDate = reader.GetSafeDateTime(offset + 19),
                InquiryType = reader.GetFieldValue<InquiryType?>(offset + 20),
                RecoveryType = reader.GetFieldValue<RecoveryType?>(offset + 21),
                RecoveryStatus = reader.GetFieldValue<RecoveryStatus?>(offset + 22),
                SurfaceArea = reader.GetSafeDouble(offset + 23),
                LivingArea = reader.GetSafeDouble(offset + 24),
                FoundationBearingLayer = reader.GetSafeDouble(offset + 25),
                RestorationCosts = reader.GetSafeDouble(offset + 26),
                FoundationType = reader.GetFieldValue<FoundationType>(offset + 27),
                FoundationTypeReliability = reader.GetFieldValue<Reliability>(offset + 28),
                Drystand = reader.GetSafeDouble(offset + 29),
                DrystandReliability = reader.GetFieldValue<Reliability>(offset + 30),
                DrystandRisk = reader.GetFieldValue<FoundationRisk>(offset + 31),
                DewateringDepth = reader.GetSafeDouble(offset + 32),
                DewateringDepthReliability = reader.GetFieldValue<Reliability>(offset + 33),
                DewateringDepthRisk = reader.GetFieldValue<FoundationRisk>(offset + 34),
                BioInfection = reader.GetSafeString(offset + 35),
                BioInfectionReliability = reader.GetFieldValue<Reliability>(offset + 36),
                BioInfectionRisk = reader.GetFieldValue<FoundationRisk>(offset + 37),
            };

        /// <summary>
        ///     Maps a reader to an <see cref="AnalysisProduct2"/>.
        /// </summary>
        public static AnalysisProduct2 MapFromReader2(DbDataReader reader, int offset = 0)
            => new()
            {
                BuildingId = reader.GetSafeString(offset++),
                ExternalBuildingId = reader.GetSafeString(offset++),
                AddressId = reader.GetSafeString(offset++),
                ExternalAddressId = reader.GetSafeString(offset++),
                NeighborhoodId = reader.GetSafeString(offset++),
                ConstructionYear = reader.GetSafeInt(offset++),
                FoundationType = reader.GetFieldValue<FoundationType>(offset++),
                FoundationTypeReliability = reader.GetFieldValue<Reliability>(offset++),
                RestorationCosts = reader.GetSafeInt(offset++),
                DrystandRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
                DrystandReliability = reader.GetFieldValue<Reliability>(offset++),
                BioInfectionRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
                BioInfectionReliability = reader.GetFieldValue<Reliability>(offset++),
                DewateringDepthRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
                DewateringDepthReliability = reader.GetFieldValue<Reliability>(offset++),
                UnclassifiedRisk = reader.GetFieldValue<FoundationRisk?>(offset++),
                RecoveryType = reader.GetFieldValue<RecoveryType?>(offset++),
            };
    }
}
