﻿using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;
using System;
using System.Data.Common;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
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
                WHERE   aa.id = @id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, aa.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its external building id and source.
        /// </summary>
        /// <remarks>
        ///     If the building is outside the geofence, an <see cref="EntityNotFoundException"/>
        ///     is thrown. Check this condition before calling this function.
        /// </remarks>
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
                WHERE   aa.external_id = upper(@external_id)";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, aa.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("external_id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its external address id and source.
        /// </summary>
        /// <remarks>
        ///     If the building is outside the geofence, an <see cref="EntityNotFoundException"/>
        ///     is thrown. Check this condition before calling this function.
        /// </remarks>
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
                WHERE   aa.address_external_id = upper(@external_id)";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, aa.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("external_id", id);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("user_id", AppContext.UserId);
            }

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
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
                RecoveryType = reader.GetFieldValue<RecoveryDocumentType?>(offset + 21),
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
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
