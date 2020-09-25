using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Repository for analysis products.
    /// </summary>
    internal sealed class AnalysisRepository : DataBase, IAnalysisRepository
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AnalysisRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        public Task<IEnumerable<AnalysisProduct>> GetAllInFenceAsync(
            Guid userId,
            INavigation navigation,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets an analysis product by its external building id and source.
        /// </summary>
        /// <remarks>
        ///     If the building is outside the geofence, an <see cref="EntityNotFoundException"/>
        ///     is thrown. Check this condition before calling this function.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="externalId">External building id.</param>
        /// <param name="externalSource">External data source</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public async Task<AnalysisProduct> GetByExternalIdAsync(
            Guid userId,
            string externalId,
            ExternalDataSource externalSource,
            CancellationToken token = default)
        {
            userId.ThrowIfNullOrEmpty();
            externalId.ThrowIfNullOrEmpty();

            // FUTURE This always returns EntityNotFoundException when the building is either not-existent or outside the fence.
            var sql = @"
                SELECT
                    ac.id,
                    ac.external_id,
                    ac.external_source,
                    ac.foundation_type,
                    ac.groundwater_level,
                    ac.foundation_risk,
                    ac.construction_year,
                    ac.building_height,
                    ac.ground_level,
                    ac.restoration_costs,
                    ac.dewatering_depth,
                    ac.drystand,
                    ac.reliability,
                    ac.neighborhood_id
                FROM data.analysis_product_complete AS ac
                WHERE
                    ac.external_id = @external_id
                    AND
                    ac.external_source = @external_source
                    AND 
                    application.is_geometry_in_fence(@user_id, ac.geom)";

            await using var connection = await DbProvider.OpenConnectionScopeAsync(token);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("external_id", externalId);
            cmd.AddParameterWithValue("external_source", externalSource);
            cmd.AddParameterWithValue("user_id", userId);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync(token);

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its internal building id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="id">Internal building id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public async Task<AnalysisProduct> GetByIdInFenceAsync(Guid userId, string id, CancellationToken token = default)
        {
            id.ThrowIfNullOrEmpty();
            userId.ThrowIfNullOrEmpty();

            // FUTURE This always returns EntityNotFoundException when the building is either not-existent or outside the fence.
            var sql = @"
                SELECT
                    ac.id,
                    ac.external_id,
                    ac.external_source,
                    ac.foundation_type,
                    ac.groundwater_level,
                    ac.foundation_risk,
                    ac.construction_year,
                    ac.building_height,
                    ac.ground_level,
                    ac.restoration_costs,
                    ac.dewatering_depth,
                    ac.drystand,
                    ac.reliability,
                    ac.neighborhood_id
                FROM data.analysis_product_complete AS ac
                WHERE
                    ac.id = @id
                    AND 
                    application.is_geometry_in_fence(@user_id, ac.geom)";

            await using var connection = await DbProvider.OpenConnectionScopeAsync(token);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            cmd.AddParameterWithValue("user_id", userId);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync(token);

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Gets an analysis product by its internal building id.
        /// </summary>
        /// <param name="id">Internal building id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public async Task<AnalysisProduct> GetByIdAsync(string id, CancellationToken token = default)
        {
            id.ThrowIfNullOrEmpty();

            // FUTURE This always returns EntityNotFoundException when the building is either not-existent.
            var sql = @"
                SELECT
                    ac.id,
                    ac.external_id,
                    ac.external_source,
                    ac.foundation_type,
                    ac.groundwater_level,
                    ac.foundation_risk,
                    ac.construction_year,
                    ac.building_height,
                    ac.ground_level,
                    ac.restoration_costs,
                    ac.dewatering_depth,
                    ac.drystand,
                    ac.reliability,
                    ac.neighborhood_id
                FROM data.analysis_product_complete AS ac
                WHERE
                    ac.id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync(token);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync(token);

            return MapFromReader(reader);
        }

        // FUTURE Sorting order and sorting column
        /// <summary>
        ///     
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="query"></param>
        /// <param name="navigation"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AnalysisProduct>> GetByQueryAsync(
            Guid userId,
            string query,
            INavigation navigation,
            CancellationToken token = default)
        {
            query.ThrowIfNullOrEmpty();
            userId.ThrowIfNullOrEmpty();

            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT
                    ac.id,
                    ac.external_id,
                    ac.external_source,
                    ac.foundation_type,
                    ac.groundwater_level,
                    ac.foundation_risk,
                    ac.construction_year,
                    ac.building_height,
                    ac.ground_level,
                    ac.restoration_costs,
                    ac.dewatering_depth,
                    ac.drystand,
                    ac.reliability,
                    ac.neighborhood_id
                FROM data.analysis_product_complete AS ac
                WHERE
                    application.is_geometry_in_fence(@user_id, ac.geom)
                    AND
                    ac.address_tsv @@ to_tsquery(@query)
                LIMIT @limit
                OFFSET @offset";

            await using var connection = await DbProvider.OpenConnectionScopeAsync(token);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("query", query);
            cmd.AddParameterWithValue("user_id", userId);
            cmd.AddParameterWithValue("limit", (navigation ?? Navigation.DefaultCollection).Limit);
            cmd.AddParameterWithValue("offset", (navigation ?? Navigation.DefaultCollection).Offset);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();

            // FUTURE: Make async enumerable.
            var result = new List<AnalysisProduct>();
            while (await reader.ReadAsync(token))
            {
                result.Add(MapFromReader(reader));
            }

            return result;
        }

        /// <summary>
        ///     Maps a reader to an <see cref="AnalysisProduct"/>.
        /// </summary>
        /// <param name="reader"><see cref="DbDataReader"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        private static AnalysisProduct MapFromReader(DbDataReader reader)
            => new AnalysisProduct
            {
                Id = reader.GetSafeString(0),
                ExternalId = reader.GetSafeString(1),
                ExternalSource = reader.GetFieldValue<ExternalDataSource>(2),
                FoundationType = reader.GetFieldValue<FoundationType>(3),
                GroundWaterLevel = reader.GetSafeDouble(4),
                FoundationRisk = reader.GetFieldValue<FoundationRisk>(5),
                ConstructionYear = reader.GetDateTime(6),
                BuildingHeight = reader.GetSafeDouble(7),
                GroundLevel = reader.GetSafeFloat(8),
                RestorationCosts = reader.GetSafeDouble(9),
                DewateringDepth = reader.GetSafeDouble(10),
                Drystand = reader.GetSafeDouble(11),
                Reliability = reader.GetFieldValue<Reliability>(12),
                NeighborhoodId = reader.GetSafeString(13)
            };
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
