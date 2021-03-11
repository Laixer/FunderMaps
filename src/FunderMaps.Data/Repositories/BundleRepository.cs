using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.MapLayer;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Repository for <see cref="Bundle"/> entities.
    /// </summary>
    internal class BundleRepository : RepositoryBase<Bundle, Guid>, IBundleRepository
    {
        /// <summary>
        ///     Creates a new <see cref="Bundle"/> in our database.
        /// </summary>
        /// <param name="entity">The bundle to create.</param>
        /// <returns>The id of the created bundle.</returns>
        public override async Task<Guid> AddAsync(Bundle entity)
        {
            var sql = @"
                INSERT INTO maplayer.bundle(
                    organization_id,
                    name,
                    layers)
                VALUES (
                    @organization_id,
                    trim(@name),
                    @layers)
                RETURNING id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            MapToWriter(context, entity);

            await using var reader = await context.ReaderAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async Task<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    maplayer.bundle";

            await using var context = await DbContextFactory.CreateAsync(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Bundle"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public override async Task DeleteAsync(Guid id)
        {
            ResetCacheEntity(id);

            var sql = @"
                DELETE
                FROM    maplayer.bundle AS b
                WHERE   b.id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Gets a <see cref="Bundle"/> by its id.
        /// </summary>
        /// <param name="id">Internal bundle id.</param>
        /// <returns>The retrieved bundle.</returns>
        public override async Task<Bundle> GetByIdAsync(Guid id)
        {
            if (TryGetEntity(id, out Bundle entity))
            {
                return entity;
            }

            var sql = @"
                SELECT  b.id,
                        b.organization_id,
                        b.name,
                        b.create_date,
                        b.update_date,
                        b.delete_date,
                        b.layer_configuration
                FROM    maplayer.bundle AS b
                WHERE   b.id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Get all existing bundles in our database.
        /// </summary>
        /// <param name="navigation">The navigation parameters.</param>
        /// <returns>Collection of bundles.</returns>
        public override async IAsyncEnumerable<Bundle> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT  b.id,
                        b.organization_id,
                        b.name,
                        b.create_date,
                        b.update_date,
                        b.delete_date,
                        b.layer_configuration
                FROM    maplayer.bundle AS b";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n WHERE b.organization_id = @id";
            }

            ConstructNavigation(sql, navigation);

            await using var context = await DbContextFactory.CreateAsync(sql);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("id", AppContext.TenantId);
            }

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Get all recent changed bundles in our database.
        /// </summary>
        /// <param name="navigation">The navigation parameters.</param>
        /// <returns>Collection of bundles.</returns>
        public async IAsyncEnumerable<Bundle> ListAllRecentAsync(Navigation navigation)
        {
            var sql = @"
                SELECT  b.id,
                        b.organization_id,
                        b.name,
                        b.create_date,
                        b.update_date,
                        b.delete_date,
                        b.layer_configuration
                FROM    maplayer.bundle AS b
                WHERE	b.create_date >= NOW() - INTERVAL '15 minutes'
                OR		b.update_date >= NOW() - INTERVAL '15 minutes'";

            // FUTURE: Maybe move up.
            if (AppContext.HasIdentity)
            {
                sql += $"\r\n WHERE b.organization_id = @id";
            }

            ConstructNavigation(sql, navigation);

            await using var context = await DbContextFactory.CreateAsync(sql);

            if (AppContext.HasIdentity)
            {
                context.AddParameterWithValue("id", AppContext.TenantId);
            }

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Updates a <see cref="Bundle"/> in our database.
        /// </summary>
        /// <param name="entity">The updated bundle.</param>
        public override async Task UpdateAsync(Bundle entity)
        {
            var sql = @"
                    UPDATE  maplayer.bundle
                    SET     name = trim(@name),
                            layer_configuration = @layer_configuration::jsonb
                    WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Maps a reader to a single <see cref="Bundle"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static Bundle MapFromReader(DbDataReader reader)
            => new()
            {
                Id = reader.GetGuid(0),
                OrganizationId = reader.GetGuid(1),
                Name = reader.GetString(2),
                CreateDate = reader.GetDateTime(3),
                UpdateDate = reader.GetSafeDateTime(4),
                DeleteDate = reader.GetSafeDateTime(5),
                LayerConfiguration = reader.GetFieldValue<LayerConfiguration>(6),
            };

        /// <summary>
        ///     Maps a single <see cref="Bundle"/> to a context.
        /// </summary>
        /// <param name="context">The context to write to.</param>
        /// <param name="entity">The entity to write.</param>
        private static void MapToWriter(DbContext context, Bundle entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            context.AddParameterWithValue("organization_id", entity.OrganizationId);
            context.AddParameterWithValue("name", entity.Name);
            context.AddJsonParameterWithValue("layer_configuration", entity.LayerConfiguration);
        }
    }
}
