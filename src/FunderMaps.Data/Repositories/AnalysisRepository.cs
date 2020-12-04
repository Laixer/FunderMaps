using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
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
    internal sealed class AnalysisRepository : DbContextBase, IAnalysisRepository
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
                        aa.address_id,
                        aa.address_external_id,
                        aa.neighborhood_id,
                        aa.groundwater_level,
                        aa.soil,
                        aa.building_height,
                        aa.ground_level,
                        aa.cpt,
                        aa.monitoring_well,
                        aa.document_name,
                        aa.document_date,
                        aa.inquiry_type,
                        aa.recovery_type,
                        aa.surface_area,
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

            await using var context = await DbContextFactory(sql);

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
                        aa.address_id,
                        aa.address_external_id,
                        aa.neighborhood_id,
                        aa.groundwater_level,
                        aa.soil,
                        aa.building_height,
                        aa.ground_level,
                        aa.cpt,
                        aa.monitoring_well,
                        aa.document_name,
                        aa.document_date,
                        aa.inquiry_type,
                        aa.recovery_type,
                        aa.surface_area,
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
                WHERE   aa.external_id = @external_id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, aa.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory(sql);

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
                        aa.address_id,
                        aa.address_external_id,
                        aa.neighborhood_id,
                        aa.groundwater_level,
                        aa.soil,
                        aa.building_height,
                        aa.ground_level,
                        aa.cpt,
                        aa.monitoring_well,
                        aa.document_name,
                        aa.document_date,
                        aa.inquiry_type,
                        aa.recovery_type,
                        aa.surface_area,
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
                WHERE   aa.address_external_id = @external_id";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n AND application.is_geometry_in_fence(@user_id, aa.geom)";
            }

            sql += $"\r\n LIMIT 1";

            await using var context = await DbContextFactory(sql);

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
                Id = reader.GetSafeString(offset + 0),
                ExternalId = reader.GetSafeString(offset + 1),
                ExternalSource = reader.GetFieldValue<ExternalDataSource>(offset + 2),
                ConstructionYear = reader.GetDateTime(offset + 3),
                AddressId = reader.GetSafeString(offset + 4),
                AddressExternalId = reader.GetSafeString(offset + 5),
                NeighborhoodId = reader.GetSafeString(offset + 6),
                GroundWaterLevel = reader.GetSafeDouble(offset + 7),
                Soil = reader.GetSafeString(offset + 8),
                BuildingHeight = reader.GetSafeDouble(offset + 9),
                GroundLevel = reader.GetSafeDouble(offset + 10),
                Cpt = reader.GetSafeString(offset + 11),
                MonitoringWell = reader.GetSafeString(offset + 12),
                DocumentName = reader.GetSafeString(offset + 13),
                DocumentDate = reader.GetSafeDateTime(offset + 14),
                InquiryType = reader.GetFieldValue<InquiryType?>(offset + 15),
                RecoveryType = reader.GetFieldValue<RecoveryDocumentType?>(offset + 16),
                SurfaceArea = reader.GetSafeDouble(offset + 17),
                FoundationBearingLayer = reader.GetSafeDouble(offset + 18),
                RestorationCosts = reader.GetSafeDouble(offset + 19),
                FoundationType = reader.GetFieldValue<FoundationType>(offset + 20),
                FoundationTypeReliability = reader.GetFieldValue<Reliability>(offset + 21),
                Drystand = reader.GetSafeDouble(offset + 22),
                DrystandReliability = reader.GetFieldValue<Reliability>(offset + 23),
                DrystandRisk = reader.GetFieldValue<FoundationRisk>(offset + 24),
                DewateringDepth = reader.GetSafeDouble(offset + 25),
                DewateringDepthReliability = reader.GetFieldValue<Reliability>(offset + 26),
                DewateringDepthRisk = reader.GetFieldValue<FoundationRisk>(offset + 27),
                BioInfection = reader.GetSafeString(offset + 28),
                BioInfectionReliability = reader.GetFieldValue<Reliability>(offset + 29),
                BioInfectionRisk = reader.GetFieldValue<FoundationRisk>(offset + 30),
            };
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
