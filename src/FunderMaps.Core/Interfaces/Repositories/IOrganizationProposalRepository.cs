using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Organization proposal repository.
    /// </summary>
    public interface IOrganizationProposalRepository : IAsyncRepository<OrganizationProposal, Guid>
    {
    }
}
