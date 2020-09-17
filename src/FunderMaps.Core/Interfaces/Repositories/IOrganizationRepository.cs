using FunderMaps.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Organization repository.
    /// </summary>
    public interface IOrganizationRepository : IAsyncRepository<Organization, Guid>
    {
        /// <summary>
        ///     Create new <see cref="Organization"/> from a proposal.
        /// </summary>
        /// <param name="id">Organization proposal identifier.</param>
        /// <param name="email">Superuser email.</param>
        /// <param name="passwordHash">Superuser password.</param>
        /// <returns>Created <see cref="Organization"/> identifier.</returns>
        ValueTask<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash);

        /// <summary>
        ///     Retrieve <see cref="Organization"/> by name.
        /// </summary>
        /// <param name="name">Organization name.</param>
        /// <returns><see cref="Organization"/>.</returns>
        ValueTask<Organization> GetByNameAsync(string name);

        /// <summary>
        ///     Retrieve <see cref="Organization"/> by email.
        /// </summary>
        /// <param name="email">Organization email.</param>
        /// <returns><see cref="Organization"/>.</returns>
        ValueTask<Organization> GetByEmailAsync(string email);
    }
}
