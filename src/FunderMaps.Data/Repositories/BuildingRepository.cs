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

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Building repository.
    /// </summary>
    internal class BuildingRepository : RepositoryBase<Building, string>, IBuildingRepository
    {
        protected override void SetCacheItem(CacheKeyPair key, Building value, MemoryCacheEntryOptions options)
        {
            options.SlidingExpiration *= 2;
            options.AbsoluteExpirationRelativeToNow *= 2;

            base.SetCacheItem(key, value, options);
        }

        public override Task<string> AddAsync(Building entity)
            => throw new NotImplementedException();

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async Task<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    geocoder.building";

            await using var context = await DbContextFactory.CreateAsync(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Incident"/>.
        /// </summary>
        /// <param name="id">Entity object.</param>
        public override Task DeleteAsync(string id)
            => throw new NotImplementedException();

        public static Building MapFromReader(DbDataReader reader, int offset = 0)
            => new()
            {
                Id = reader.GetSafeString(offset + 0),
                BuildingType = reader.GetFieldValue<BuildingType?>(offset + 1),
                BuiltYear = reader.GetSafeDateTime(offset + 2),
                IsActive = reader.GetBoolean(offset + 3),
                ExternalId = reader.GetSafeString(offset + 4),
                ExternalSource = reader.GetFieldValue<ExternalDataSource>(offset + 5),
                Geometry = reader.GetString(offset + 6),
                NeighborhoodId = reader.GetSafeString(offset + 7),
            };

        public override async Task<Building> GetByIdAsync(string id)
        {
            if (TryGetEntity(id, out Building entity))
            {
                return entity;
            }

            var sql = @"
                SELECT  -- Building
                        b.id,
                        b.building_type,
                        b.built_year,
                        b.is_active,
                        b.external_id,
                        b.external_source,
                        b.geom,
                        b.neighborhood_id
                FROM    geocoder.building_encoded_geom AS b
                WHERE   b.id = @id
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Checks whether or not an item exists inside the geofence of the
        ///     organization to which the <paramref name="userId"/> belongs.
        /// </summary>
        /// <remarks>
        ///     If said organization has no fence this will return true.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="buildingId">Internal building id.</param>
        /// <returns>Boolean result</returns>
        public Task<bool> IsInGeoFenceAsync(Guid userId, string buildingId)
            => throw new NotImplementedException();

        public override IAsyncEnumerable<Building> ListAllAsync(INavigation navigation)
            => throw new NotImplementedException();

        public override Task UpdateAsync(Building entity)
            => throw new NotImplementedException();
    }
}
