using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Operations for the organization proposal repository.
    /// </summary>
    public interface IOrganizationProposalRepository : IAsyncRepository<OrganizationProposal, Guid>
    {
        /// <summary>
        /// Retrieve entity by name.
        /// </summary>
        /// <param name="name">Organization name.</param>
        /// <returns><see cref="OrganizationProposal"/> on success, null on error.</returns>
        Task<OrganizationProposal> GetByNormalizedNameAsync(string name);
    }
}
