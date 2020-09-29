using FunderMaps.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Operations for the building repository.
    /// </summary>
    public interface IBuildingRepository : IAsyncRepository<Building, string>
    {
        /// <summary>
        ///     Checks whether or not a building lies in a users geofence.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        /// <param name="buildingId">Internal building id</param>
        /// <returns>Boolean result</returns>
        Task<bool> IsInGeoFenceAsync(Guid userId, string buildingId);
    }
}
