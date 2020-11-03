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
                    user_id,
                    layers)
                VALUES (
                    @organization_id,
                    @name,
                    @user_id,
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
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(Guid id)
        {
            ResetCacheEntity(id);

            var sql = @"
                DELETE
                FROM    maplayer.bundle
                WHERE   id = @id";

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
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var sql = @"
                SELECT  id,
                        organization_id,
                        name,
                        create_date,
                        update_date,
                        delete_date,
                        user_id,
                        layer_configuration,
                        version_id,
                        bundle_status
                FROM    maplayer.bundle
                WHERE   id = @id";

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
                SELECT  id,
                        organization_id,
                        name,
                        create_date,
                        update_date,
                        delete_date,
                        user_id,
                        layer_configuration,
                        version_id,
                        bundle_status
                FROM    maplayer.bundle";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Get all bundles for a given organization.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="navigation">Navigation.</param>
        /// <returns>Collection of <see cref="Bundle"/> entities.</returns>
        public async IAsyncEnumerable<Bundle> ListAllByOrganizationAsync(Guid organizationId, INavigation navigation)
        {
            if (organizationId == null || organizationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(organizationId));
            }
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        organization_id,
                        name,
                        create_date,
                        update_date,
                        delete_date,
                        user_id,
                        layer_configuration,
                        version_id,
                        bundle_status
                FROM    maplayer.bundle
                WHERE   organization_id = @organization_id";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("organization_id", organizationId);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Marks a bundle as processing.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        public async Task MarkAsProcessingAsync(Guid bundleId)
        {
            bundleId.ThrowIfNullOrEmpty();

            var sql = @"
                    UPDATE  maplayer.bundle
                    SET     bundle_status = @bundle_status
                    WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", bundleId);
            context.AddParameterWithValue("bundle_status", BundleStatus.Processing);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Marks a bundle as up to date and bumps the version id.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        public async Task MarkAsUpToDateBumpVersionAsync(Guid bundleId)
        {
            bundleId.ThrowIfNullOrEmpty();

            var sql = @"
                    UPDATE  maplayer.bundle
                    SET     bundle_status = @bundle_status,
                            version_id = version_id + 1
                    WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", bundleId);
            context.AddParameterWithValue("bundle_status", BundleStatus.UpToDate);

            await context.NonQueryAsync();
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
                UserId = reader.GetGuid(6),
                LayerConfiguration = reader.GetFieldValue<LayerConfiguration>(7),
                VersionId = reader.GetUInt(8), // TODO Is uint correct? DB = int4
                BundleStatus = reader.GetFieldValue<BundleStatus>(9)
            };

        /// <summary>
        ///     Maps a single <see cref="Bundle"/> to a command.
        /// </summary>
        /// <param name="cmd">The command to write to.</param>
        /// <param name="entity">The entity to write.</param>
        private static void MapToWriter(DbContext context, Bundle entity)
        {
            context.AddParameterWithValue("organization_id", entity.OrganizationId);
            context.AddParameterWithValue("name", entity.Name);
            context.AddParameterWithValue("user_id", entity.UserId);
            context.AddJsonParameterWithValue("layer_configuration", entity.LayerConfiguration);
        }
    }
}
