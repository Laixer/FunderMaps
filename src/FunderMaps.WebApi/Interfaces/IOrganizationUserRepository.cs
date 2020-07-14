using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Models.Identity;
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
        Task<IReadOnlyList<OrganizationUser>> ListAllByOrganizationIdAsync(Guid orgId, INavigation navigation);

        /// <summary>
        /// Retrieve entities by organization id and by role.
        /// </summary>
        /// <param name="role">Role to select.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <param name="navigation">Recordset navigation.</param>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<FunderMapsUser>> ListAllByOrganizationByRoleIdAsync(OrganizationRole role, Guid orgId, INavigation navigation);

        /// <summary>
        /// Retrieve entity by user id.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>List of entities.</returns>
        Task<OrganizationUser> GetByUserIdAsync(Guid userId);
    }
}
