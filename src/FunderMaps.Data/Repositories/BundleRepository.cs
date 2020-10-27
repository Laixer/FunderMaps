using FunderMaps.Core.Entities;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.MapLayer;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Repository for <see cref="Bundle"/> entities.
    /// </summary>
    internal class BundleRepository : DataBase, IBundleRepository
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="Bundle"/> in our database.
        /// </summary>
        /// <param name="entity">The bundle to create.</param>
        /// <returns>The id of the created bundle.</returns>
        public async ValueTask<Guid> AddAsync(Bundle entity)
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
                RETURNING id;
            ";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            MapToWriter(cmd, entity);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();
            return reader.GetGuid(0);
        }

        public ValueTask<ulong> CountAsync() => throw new NotImplementedException();

        public ValueTask DeleteAsync(Guid id) => throw new NotImplementedException();

        /// <summary>
        ///     Gets a <see cref="Bundle"/> by its id.
        /// </summary>
        /// <param name="id">Internal bundle id.</param>
        /// <returns>The retrieved bundle.</returns>
        public async ValueTask<Bundle> GetByIdAsync(Guid id)
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

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Get all existing bundles in our database.
        /// </summary>
        /// <param name="navigation">The navigation parameters.</param>
        /// <returns>Collection of bundles.</returns>
        public async IAsyncEnumerable<Bundle> ListAllAsync(INavigation navigation)
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

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Get all bundles for a given organization.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="navigation">Navigation.</param>
        /// <returns>Collection of <see cref="Bundle"/> entities.</returns>
        public async IAsyncEnumerable<Bundle> GetAllByOrganizationAsync(Guid organizationId, INavigation navigation)
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

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("organization_id", organizationId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Marks a bundle as processing.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        public Task MarkAsProcessingAsync(Guid bundleId)
        {
            bundleId.ThrowIfNullOrEmpty();
            return MarkAsStatusAsync(bundleId, BundleStatus.Processing);
        }

        /// <summary>
        ///     Marks a bundle as up to date.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        public Task MarkAsUpToDateAsync(Guid bundleId)
        {
            bundleId.ThrowIfNullOrEmpty();
            return MarkAsStatusAsync(bundleId, BundleStatus.UpToDate);
        }

        /// <summary>
        ///     Updates a <see cref="Bundle"/> in our database.
        /// </summary>
        /// <param name="entity">The updated bundle.</param>
        /// <returns>See <see cref="ValueTask"/>.</returns>
        public async ValueTask UpdateAsync(Bundle entity)
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


            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("id", entity.Id);
            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
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
        private static void MapToWriter(DbCommand cmd, Bundle entity)
        {
            cmd.AddParameterWithValue("organization_id", entity.OrganizationId);
            cmd.AddParameterWithValue("name", entity.Name);
            cmd.AddParameterWithValue("user_id", entity.UserId);
            cmd.AddParameterWithValue("layer_configuration", JsonSerializer.Serialize(entity.LayerConfiguration));
        }

        /// <summary>
        ///     Marks a bundle with a given status.
        /// </summary>
        /// <param name="bundleId">The bundle id to mark.</param>
        /// <param name="bundleStatus">The new bundle status.</param>
        private async Task MarkAsStatusAsync(Guid bundleId, BundleStatus bundleStatus)
        {
            var sql = @"
                    UPDATE  maplayer.bundle
                    SET     bundle_status = @bundle_status
                    WHERE   id = @id";


            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("id", bundleId);
            cmd.AddParameterWithValue("bundle_status", bundleStatus);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }
    }
}
