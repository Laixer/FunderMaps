using FunderMaps.Core.Entities;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Building repository.
    /// </summary>
    internal class BuildingRepository : RepositoryBase<Building, string>, IBuildingRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public BuildingRepository(DbProvider dbProvider) : base(dbProvider) { }

        public override ValueTask<string> AddAsync(Building entity) => throw new NotImplementedException();

        public override ValueTask<ulong> CountAsync() => throw new NotImplementedException();

        public override ValueTask DeleteAsync(string id) => throw new NotImplementedException();

        public override ValueTask<Building> GetByIdAsync(string id) => throw new NotImplementedException();

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
        public Task<bool> IsInGeoFenceAsync(Guid userId, string buildingId, CancellationToken token)
        {
            userId.ThrowIfNullOrEmpty();
            buildingId.ThrowIfNullOrEmpty();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            throw new NotImplementedException();
        }

        public override IAsyncEnumerable<Building> ListAllAsync(INavigation navigation) => throw new NotImplementedException();

        public override ValueTask UpdateAsync(Building entity) => throw new NotImplementedException();
    }
}
