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
        public Task<IEnumerable<AnalysisProduct>> GetAllInFenceAsync(Guid userId, INavigation navigation, CancellationToken token = default) => throw new NotImplementedException();

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
        public async Task<AnalysisProduct> GetByExternalIdAsync(Guid userId, string externalId, ExternalDataSource externalSource, CancellationToken token = default)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            externalId.ThrowIfNullOrEmpty();

            // Build sql.
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
                    ac.external_id = @ExternalId
                    AND
                    ac.external_source = @ExternalSource
                    AND 
                    application.is_geometry_in_fence(@UserId, ac.geom)";

            // Execute sql.
            await using var connection = await DbProvider.OpenConnectionScopeAsync(token);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("ExternalId", externalId);
            cmd.AddParameterWithValue("ExternalSource", externalSource);
            cmd.AddParameterWithValue("UserId", userId);

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
        public async Task<AnalysisProduct> GetByIdAsync(Guid userId, string id, CancellationToken token = default)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            id.ThrowIfNullOrEmpty();

            // Build sql.
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
                    ac.id = @Id
                    AND 
                    application.is_geometry_in_fence(@UserId, ac.geom)";

            // Execute sql.
            await using var connection = await DbProvider.OpenConnectionScopeAsync(token);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("Id", id);
            cmd.AddParameterWithValue("UserId", userId);

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
        public async Task<IEnumerable<AnalysisProduct>> GetByQueryAsync(Guid userId, string query, INavigation navigation, CancellationToken token = default)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            query.ThrowIfNullOrEmpty();
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            // Build sql.
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
                    application.is_geometry_in_fence(@UserId, ac.geom)
                    AND
                    ac.address_tsv @@ to_tsquery(@Query)
                LIMIT @Limit
                OFFSET @Offset";

            // Execute sql.
            await using var connection = await DbProvider.OpenConnectionScopeAsync(token);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("Query", query);
            cmd.AddParameterWithValue("UserId", userId);
            cmd.AddParameterWithValue("Limit", (navigation ?? Navigation.DefaultCollection).Limit);
            cmd.AddParameterWithValue("Offset", (navigation ?? Navigation.DefaultCollection).Offset);

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
