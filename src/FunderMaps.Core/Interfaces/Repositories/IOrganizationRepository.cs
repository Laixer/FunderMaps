using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    public interface IOrganizationRepository : IAsyncRepository<Organization, Guid>
    {
        ValueTask<Guid> AddProposalAsync(OrganizationProposal organization);

        ValueTask<OrganizationProposal> GetProposalByIdAsync(Guid id);

        ValueTask<OrganizationProposal> GetProposalByNameAsync(string name);

        ValueTask<OrganizationProposal> GetProposalByEmailAsync(string email);

        IAsyncEnumerable<OrganizationProposal> ListAllProposalAsync(INavigation navigation);

        ValueTask DeleteProposalAsync(Guid id);

        ValueTask<Organization> GetByNameAsync(string name);

        ValueTask<Organization> GetByEmailAsync(string email);
    }
}
