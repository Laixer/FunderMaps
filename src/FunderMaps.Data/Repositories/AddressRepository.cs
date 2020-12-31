using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Address repository.
    /// </summary>
    internal class AddressRepository : RepositoryBase<Address, string>, IAddressRepository
    {
        protected override void SetCacheItem(KeyPair key, Address value, MemoryCacheEntryOptions options)
        {
            options.SlidingExpiration *= 2;
            options.AbsoluteExpirationRelativeToNow *= 2;

            base.SetCacheItem(key, value, options);
        }

        public static void MapToWriter(DbContext context, Address entity)
        {
            context.AddParameterWithValue("building_number", entity.BuildingNumber);
            context.AddParameterWithValue("postal_code", entity.PostalCode);
            context.AddParameterWithValue("street", entity.Street);
            context.AddParameterWithValue("is_active", entity.IsActive);
            context.AddParameterWithValue("external_id", entity.ExternalId);
            context.AddParameterWithValue("external_source", entity.ExternalSource);
        }

        public static Address MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new()
            {
                Id = reader.GetSafeString(offset + 0),
                BuildingNumber = reader.GetSafeString(offset + 1),
                PostalCode = reader.GetSafeString(offset + 2),
                Street = reader.GetSafeString(offset + 3),
                IsActive = reader.GetBoolean(offset + 4),
                ExternalId = reader.GetSafeString(offset + 5),
                ExternalSource = reader.GetFieldValue<ExternalDataSource>(offset + 6),
                City = reader.GetSafeString(offset + 7),
                BuildingId = reader.GetSafeString(offset + 8),
                BuildingNavigation = fullMap ? BuildingRepository.MapFromReader(reader, offset + 9) : null,
            };

        /// <summary>
        ///     Create new <see cref="Address"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Address"/>.</returns>
        public override async ValueTask<string> AddAsync(Address entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    INSERT INTO geocoder.address(
                        building_number,
                        postal_code,
                        street,
                        is_active,
                        external_id,
                        external_source)
                    VALUES (
                        @building_number,
                        @postal_code,
                        @street,
                        @is_active,
                        upper(@external_id),
                        @external_source)
                    ON CONFLICT DO NOTHING
                    RETURNING id";

            await using var context = await DbContextFactory(sql);

            MapToWriter(context, entity);

            await using var reader = await context.ReaderAsync();

            return reader.GetSafeString(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async ValueTask<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    geocoder.address";

            await using var context = await DbContextFactory(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Incident"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public override ValueTask DeleteAsync(string id)
            => throw new InvalidOperationException();

        public async ValueTask<Address> GetByExternalIdAsync(string id, ExternalDataSource source)
        {
            var sql = @"
                SELECT  -- Address
                        a.id,
                        a.building_number,
                        a.postal_code,
                        a.street,
                        a.is_active,
                        a.external_id,
                        a.external_source,
                        a.city,
                        a.building_id,

                        -- Building
                        b.id,
                        b.building_type,
                        b.built_year,
                        b.is_active,
                        b.external_id, 
                        b.external_source, 
                        b.geom,
                        b.neighborhood_id
                FROM    geocoder.address AS a
                JOIN    geocoder.building_encoded_geom AS b ON b.id = a.building_id
                WHERE   a.external_id = upper(@external_id)
                AND     a.external_source = @external_source
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("external_id", id);
            context.AddParameterWithValue("external_source", source);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader, fullMap: true));
        }

        /// <summary>
        ///     Retrieve <see cref="Address"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Address"/>.</returns>
        public override async ValueTask<Address> GetByIdAsync(string id)
        {
            if (TryGetEntity(id, out Address entity))
            {
                return entity;
            }

            var sql = @"
                SELECT  -- Address
                        a.id,
                        a.building_number,
                        a.postal_code,
                        a.street,
                        a.is_active,
                        a.external_id,
                        a.external_source,
                        a.city,
                        a.building_id,

                        -- Building
                        b.id,
                        b.building_type,
                        b.built_year,
                        b.is_active,
                        b.external_id, 
                        b.external_source, 
                        b.geom,
                        b.neighborhood_id
                FROM    geocoder.address AS a
                JOIN    geocoder.building_encoded_geom AS b ON b.id = a.building_id
                WHERE   a.id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader, fullMap: true));
        }

        /// <summary>
        ///     Retrieve <see cref="Address"/> by search query.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <param name="navigation">The navigation parameters.</param>
        /// <returns><see cref="Address"/>.</returns>
        public async IAsyncEnumerable<Address> GetBySearchQueryAsync(string query, INavigation navigation)
        {
            if (navigation is null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  -- Address
                        a.id,
                        a.building_number,
                        a.postal_code,
                        a.street,
                        a.is_active,
                        a.external_id,
                        a.external_source,
                        a.city,
                        a.building_id,

                        -- Building
                        b.id,
                        b.building_type,
                        b.built_year,
                        b.is_active,
                        b.external_id, 
                        b.external_source, 
                        b.geom,
                        b.neighborhood_id
                FROM    geocoder.search_address(@query) AS a
                JOIN    geocoder.building_encoded_geom AS b ON b.id = a.building_id";

            ConstructNavigation(ref sql, navigation, "a");

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("query", query);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader, fullMap: true));
            }
        }

        /// <summary>
        ///     Retrieve all <see cref="Address"/>.
        /// </summary>
        /// <returns>List of <see cref="Address"/>.</returns>
        public override async IAsyncEnumerable<Address> ListAllAsync(INavigation navigation)
        {
            if (navigation is null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  -- Address
                        a.id,
                        a.building_number,
                        a.postal_code,
                        a.street,
                        a.is_active,
                        a.external_id,
                        a.external_source,
                        a.city,
                        a.building_id,

                        -- Building
                        b.id,
                        b.building_type,
                        b.built_year,
                        b.is_active,
                        b.external_id, 
                        b.external_source, 
                        b.geom,
                        b.neighborhood_id
                FROM    geocoder.address AS a
                JOIN    geocoder.building_encoded_geom AS b ON b.id = a.building_id";

            ConstructNavigation(ref sql, navigation, "a");

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader, fullMap: true));
            }
        }

        /// <summary>
        ///     Update <see cref="Address"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(Address entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            ResetCacheEntity(entity);

            var sql = @"
                    UPDATE  geocoder.address
                    SET     building_number = @building_number,
                            postal_code = @postal_code,
                            street = @street,
                            is_active = @is_active,
                            external_id = upper(@external_id),
                            external_source = @external_source
                    WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
