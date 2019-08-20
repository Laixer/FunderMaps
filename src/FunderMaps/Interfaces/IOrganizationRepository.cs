using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Interfaces
{
    public interface IOrganizationRepository : IAsyncRepository<Organization, int>
    {
        /// <summary>
        /// Get existing principal entity from data store or insert given
        /// as new entity.
        /// </summary>
        /// <param name="organization">Input organization.</param>
        /// <returns>See <see cref="Organization"/>.</returns>
        Task<Organization> GetOrAddAsync(Organization organization);
    }
}
