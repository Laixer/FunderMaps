using FunderMaps.Core.Entities;
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
        /// <param name="navigation">Recordset navigation.</param>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<OrganizationUser>> ListAllByOrganizationIdAsync(Guid orgId, Navigation navigation);

        /// <summary>
        /// Retrieve entities by organization id and by role.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <param name="navigation">Recordset navigation.</param>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<OrganizationUser>> ListAllByOrganizationByRoleIdAsync(OrganizationRole role, Guid orgId, Navigation navigation);

        /// <summary>
        /// Retrieve entity by user id.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of entities.</returns>
        Task<OrganizationUser> GetByUserIdAsync(Guid userId);
    }
}
