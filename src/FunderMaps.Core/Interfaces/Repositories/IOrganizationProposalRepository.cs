using FunderMaps.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    public interface IOrganizationProposalRepository : IAsyncRepository<OrganizationProposal, Guid>
    {
        ValueTask<OrganizationProposal> GetByNameAsync(string name);

        ValueTask<OrganizationProposal> GetByEmailAsync(string email);
    }
}
