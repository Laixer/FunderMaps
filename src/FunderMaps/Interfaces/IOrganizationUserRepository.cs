using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Organization user repository.
    /// </summary>
    public interface IOrganizationUserRepository : IAsyncRepository<OrganizationUser, KeyValuePair<Guid, Guid>>
    {
        /// <summary>
        /// Retrieve entities by organization id.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<OrganizationUser>> ListAllByOrganizationIdAsync(Guid orgId, Navigation navigation);
    }
}
