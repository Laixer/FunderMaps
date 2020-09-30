using FunderMaps.Core.Entities;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Building repository.
    /// </summary>
    internal class BuildingRepository : RepositoryBase<Building, string>, IBuildingRepository
    {
        public override ValueTask<string> AddAsync(Building entity)
            => throw new NotImplementedException();

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    geocoder.building";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        ///     Delete <see cref="Incident"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override ValueTask DeleteAsync(string id)
            => throw new NotImplementedException();

        private static Building MapFromReader(DbDataReader reader)
            => new Building
            {
                Id = reader.GetSafeString(0),
                BuildingType = reader.GetFieldValue<BuildingType?>(1),
                BuiltYear = reader.GetDateTime(1),
                IsActive = reader.GetBoolean(3),
                ExternalId = reader.GetSafeString(4),
                ExternalSource = reader.GetFieldValue<ExternalDataSource>(5),
                Geometry = reader.GetString(6),
                NeighborhoodId = reader.GetSafeString(7),
            };

        public override async ValueTask<Building> GetByIdAsync(string id)
        {
            var sql = @"
                SELECT  id,
                        building_type,
                        built_year,
                        is_active,
                        external_id,
                        external_source,
                        geom,
                        neighborhood_id
                FROM    geocoder.building_encoded_geom
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
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
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Boolean result</returns>
        public Task<bool> IsInGeoFenceAsync(Guid userId, string buildingId)
        {
            userId.ThrowIfNullOrEmpty();
            buildingId.ThrowIfNullOrEmpty();

            throw new NotImplementedException();
        }

        public override IAsyncEnumerable<Building> ListAllAsync(INavigation navigation)
            => throw new NotImplementedException();

        public override ValueTask UpdateAsync(Building entity)
            => throw new NotImplementedException();
    }
}
