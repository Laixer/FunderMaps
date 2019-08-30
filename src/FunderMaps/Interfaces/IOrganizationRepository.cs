using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Models.Identity;
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
        /// Get organization by user.
        /// </summary>
        /// <param name="user">The user to get organization by.</param>
        /// <returns>Organization or null.</returns>
        Task<Organization> GetOrganizationAsync(FunderMapsUser user);

        /// <summary>
        /// Get user role in organization.
        /// </summary>
        /// <param name="organization">The organization to find the role by</param>
        /// <param name="user">The user to find the role by.</param>
        /// <returns>User role or null.</returns>
        Task<string> GetRoleAsync(Organization organization, FunderMapsUser user);
    }
}
