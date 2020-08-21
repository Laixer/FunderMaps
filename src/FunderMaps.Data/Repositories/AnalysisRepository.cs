using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
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
        private readonly IDescriptionService _descriptionService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AnalysisRepository(DbProvider dbProvider,
            IDescriptionService descriptionService)
            : base(dbProvider) => _descriptionService = descriptionService ?? throw new ArgumentNullException(nameof(descriptionService));

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        public Task<IEnumerable<AnalysisProduct>> GetAllInFenceAsync(Guid userId, INavigation navigation, CancellationToken token) => throw new NotImplementedException();

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
        public async Task<AnalysisProduct> GetByExternalIdAsync(Guid userId, string externalId, ExternalDataSource externalSource, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            externalId.ThrowIfNullOrEmpty();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Build sql.
            // TODO This always returns EntityNotFoundException when the building is either not-existent or outside the fence.
            var sql = @"
                SELECT
                    *
                FROM data.analysis_complete AS ac
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

            // Map, append and return product.
            var product = MapFromReader(reader);
            product.FullDescription = _descriptionService.GenerateFullDescription(product);
            product.TerrainDescription = _descriptionService.GenerateTerrainDescription(product);
            return product;
        }

        /// <summary>
        ///     Gets an analysis product by its internal building id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="id">Internal building id.</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public async Task<AnalysisProduct> GetByIdAsync(Guid userId, string id, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            id.ThrowIfNullOrEmpty();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Build sql.
            // TODO This always returns EntityNotFoundException when the building is either not-existent or outside the fence.
            var sql = @"
                SELECT
                    *
                FROM data.analysis_complete AS ac
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

            // Map, append and return product.
            var product = MapFromReader(reader);
            product.FullDescription = _descriptionService.GenerateFullDescription(product);
            product.TerrainDescription = _descriptionService.GenerateTerrainDescription(product);
            return product;
        }

        /// FUTURE Sorting order and sorting column
        /// <summary>
        ///     
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="query"></param>
        /// <param name="navigation"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AnalysisProduct>> GetByQueryAsync(Guid userId, string query, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            query.ThrowIfNullOrEmpty();
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Build sql.
            var sql = @"
                SELECT
                    *
                FROM data.analysis_complete AS ac
                JOIN geocoder.address AS a ON a.building_id = ac.id
                JOIN geocoder.search_address(@Query) AS s ON s.id = a.id
                WHERE
                    application.is_geometry_in_fence(@UserId, ac.geom)
                LIMIT @Limit
                OFFSET @Offset";

            // Execute sql.
            await using var connection = await DbProvider.OpenConnectionScopeAsync(token);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("Query", query);
            cmd.AddParameterWithValue("UserId", userId);
            cmd.AddParameterWithValue("Limit", (navigation ?? Navigation.DefaultCollection).Limit);
            cmd.AddParameterWithValue("Offset", (navigation ?? Navigation.DefaultCollection).Limit);
            
            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();

            // Map, append and return products.
            // FUTURE: Make async enumerable.
            var result = new List<AnalysisProduct>();
            while (await reader.ReadAsync(token))
            {
                var product = MapFromReader(reader);
                product.FullDescription = _descriptionService.GenerateFullDescription(product);
                product.TerrainDescription = _descriptionService.GenerateTerrainDescription(product);
                result.Add(product);
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
                GroundWaterLevel = reader.GetDouble(4),
                FoundationRisk = reader.GetFieldValue<FoundationRisk>(5),
                ConstructionYear = DateTimeOffsetHelper.FromYear(reader.GetSafeInt(6) ?? 0), // FUTURE Make extension, clean up
                BuildingHeight = reader.GetDouble(7),
                GroundLevel = reader.GetFloat(8),
                RestorationCosts = reader.GetDouble(9),
                DewateringDepth = reader.GetDouble(10),
                Drystand = reader.GetDouble(11),
                Reliability = reader.GetFieldValue<Reliability>(12),
                NeighborhoodId = reader.GetSafeString(13)
            };
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
