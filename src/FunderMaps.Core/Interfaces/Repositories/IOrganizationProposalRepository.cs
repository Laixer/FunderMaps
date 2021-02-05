using FunderMaps.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Organization proposal repository.
    /// </summary>
    public interface IOrganizationProposalRepository : IAsyncRepository<OrganizationProposal, Guid>
    {
        /// <summary>
        ///     Retrieve <see cref="OrganizationProposal"/> by name.
        /// </summary>
        /// <param name="name">Organization name.</param>
        /// <returns><see cref="OrganizationProposal"/>.</returns>
        Task<OrganizationProposal> GetByNameAsync(string name);

        /// <summary>
        ///     Retrieve <see cref="OrganizationProposal"/> by email.
        /// </summary>
        /// <param name="email">Organization email.</param>
        /// <returns><see cref="OrganizationProposal"/>.</returns>
        Task<OrganizationProposal> GetByEmailAsync(string email);
    }
}
