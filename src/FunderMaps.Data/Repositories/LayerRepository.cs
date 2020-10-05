using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Repository implementation for <see cref="Layer"/> entities.
    /// </summary>
    internal class LayerRepository : DataBase, ILayerRepository
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public LayerRepository(DbProvider dbProvider)
          : base(dbProvider)
        {
        }

        /// <summary>
        ///     Retrieve layer by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Layer"/>.</returns>
        public async ValueTask<Layer> GetByIdAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var sql = @"
                SELECT  id,
                        name
                FROM    maplayer.layer
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve all layers that are linked with a <paramref name="bundleId"/>.
        /// </summary>
        /// <param name="bundleId">The linked bundle id.</param>
        public async IAsyncEnumerable<Layer> ListAllFromBundleIdAsync(Guid bundleId)
        {
            if (bundleId == null || bundleId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bundleId));
            }

            var sql = @"
                SELECT  l.id,
                        l.name
                FROM    maplayer.layer AS l
                JOIN    maplayer.bundle AS b ON l.id = ANY(b.layers)
                WHERE   b.id = @bundle_id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("bundle_id", bundleId);

            await using var reader = await cmd.ExecuteReaderCanHaveZeroRowsAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Retrieve all layers.
        /// </summary>
        /// <returns>List of <see cref="Layer"/>.</returns>
        public async IAsyncEnumerable<Layer> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        name
                FROM    maplayer.layer";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderCanHaveZeroRowsAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Maps a reader to a single <see cref="Layer"/>.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static Layer MapFromReader(DbDataReader reader)
            => new Layer
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1)
            };
    }
}
