using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Repository implementation for <see cref="Layer"/> entities.
    /// </summary>
    internal class LayerRepository : RepositoryBase<Layer, Guid>, ILayerRepository
    {
        /// <summary>
        ///     Create new <see cref="Layer"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Layer"/>.</returns>
        public override async ValueTask<Guid> AddAsync(Layer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO maplayer.layer(
                    schema_name,
                    table_name)
                VALUES (
                    @schema_name,
                    @table_name)
                RETURNING id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("schema_name", entity.SchemaName);
            context.AddParameterWithValue("table_name", entity.TableName);

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
                FROM    maplayer.layer";

            await using var context = await DbContextFactory(sql);

            return await context.ScalarAsync<long>();
        }
        /// <summary>
        ///     Delete <see cref="Layer"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(Guid id)
        {
            ResetCacheEntity(id);

            var sql = @"
                DELETE
                FROM    maplayer.layer
                WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Retrieve <see cref="Layer"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Layer"/>.</returns>
        public override async ValueTask<Layer> GetByIdAsync(Guid id)
        {
            var sql = @"
                SELECT  id,
                        schema_name,
                        table_name
                FROM    maplayer.layer
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Retrieve all layers that are linked with a <paramref name="bundleId"/>.
        /// </summary>
        /// <param name="bundleId">The linked bundle id.</param>
        public async IAsyncEnumerable<Layer> ListAllFromBundleIdAsync(Guid bundleId)
        {
            var sql = @"
                WITH layer_ids AS (
	                SELECT (jsonb_array_elements(layer_configuration ->'Layers')->>'LayerId')::uuid AS layer_id
	                FROM maplayer.bundle b 
                    WHERE b.id = @id
                )
                SELECT  l.id,
                        l.schema_name,
                        l.table_name
                FROM    maplayer.layer AS l
                JOIN    maplayer.bundle AS b ON l.id = ANY(
                            SELECT  layer_id 
                            FROM    layer_ids
                        )
                WHERE   b.id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", bundleId);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Retrieve all layers.
        /// </summary>
        /// <returns>List of <see cref="Layer"/>.</returns>
        public override async IAsyncEnumerable<Layer> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        schema_name,
                        table_name
                FROM    maplayer.layer";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }
        /// <summary>
        ///     Update <see cref="Layer"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(Layer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            ResetCacheEntity(entity);

            var sql = @"
                    UPDATE  maplayer.layer
                    SET     schema_name = @schema_name,
                            table_name = @table_name
                    WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);
            context.AddParameterWithValue("schema_name", entity.SchemaName);
            context.AddParameterWithValue("table_name", entity.TableName);

            await context.NonQueryAsync();
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
                SchemaName = reader.GetString(1),
                TableName = reader.GetString(2)
            };
    }
}
