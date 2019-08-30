using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Interfaces;
using FunderMaps.Models.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Organization repository.
    /// </summary>
    public interface IOrganizationRepository : IAsyncRepository<Organization, int>
    {
        /// <summary>
        /// Get existing principal entity from data store or insert given
        /// as new entity.
        /// </summary>
        /// <param name="organization">Input organization.</param>
        /// <returns>See <see cref="Organization"/>.</returns>
        Task<Organization> GetOrAddAsync(Organization organization);

        /// <summary>
        /// Get all organizations this user is part of.
        /// </summary>
        /// <param name="user">The user to get organizations by.</param>
        /// <returns>List of organizations.</returns>
        Task<IReadOnlyList<Organization>> GetAllOrganizationsAsync(FunderMapsUser user);

        /// <summary>
        /// Get user role in organization.
        /// </summary>
        /// <param name="organization">The organization to find the role by</param>
        /// <param name="user">The user to find the role by.</param>
        /// <returns>User role or null.</returns>
        Task<string> GetRoleAsync(Organization organization, FunderMapsUser user);
    }
}
