using FunderMaps.Core.Entities;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
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
        public override async ValueTask<Guid> AddAsync(Bundle entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO maplayer.bundle(
                    organization_id,
                    name,
                    layers)
                VALUES (
                    @organization_id,
                    @name,
                    @layers)
                RETURNING id";

            await using var context = await DbContextFactory(sql);

            MapToWriter(context, entity);

            await using var reader = await context.ReaderAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async ValueTask<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    maplayer.bundle";

            await using var context = await DbContextFactory(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Bundle"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public override async ValueTask DeleteAsync(Guid id)
        {
            ResetCacheEntity(id);

            var sql = @"
                DELETE
                FROM    maplayer.bundle AS b
                WHERE   b.id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Gets a <see cref="Bundle"/> by its id.
        /// </summary>
        /// <param name="id">Internal bundle id.</param>
        /// <returns>The retrieved bundle.</returns>
        public override async ValueTask<Bundle> GetByIdAsync(Guid id)
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Get all existing bundles in our database.
        /// </summary>
        /// <param name="navigation">The navigation parameters.</param>
        /// <returns>Collection of bundles.</returns>
        public override async IAsyncEnumerable<Bundle> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

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

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

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
        /// <returns>See <see cref="ValueTask"/>.</returns>
        public override async ValueTask UpdateAsync(Bundle entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  maplayer.bundle
                    SET     name = @name,
                            layer_configuration = @layer_configuration::jsonb
                    WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

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
            => new Bundle
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
            context.AddParameterWithValue("organization_id", entity.OrganizationId);
            context.AddParameterWithValue("name", entity.Name);
            context.AddJsonParameterWithValue("layer_configuration", entity.LayerConfiguration);
        }
    }
}
