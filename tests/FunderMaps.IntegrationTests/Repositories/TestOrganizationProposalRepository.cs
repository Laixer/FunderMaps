using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class TestOrganizationProposalRepository : TestRepositoryBase<OrganizationProposal, Guid>, IOrganizationProposalRepository
    {
        public TestOrganizationProposalRepository(EntityDataStore<OrganizationProposal> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override ValueTask<Guid> AddAsync(OrganizationProposal entity)
        {
            entity.Id = Guid.NewGuid();
            return base.AddAsync(entity);
        }

        public ValueTask<OrganizationProposal> GetByEmailAsync(string email)
        {
            return new ValueTask<OrganizationProposal>(DataStore.Entities.FirstOrDefault(e => e.Email == email));
        }

        public ValueTask<OrganizationProposal> GetByNameAsync(string name)
        {
            return new ValueTask<OrganizationProposal>(DataStore.Entities.FirstOrDefault(e => e.Name == name));
        }
    }
}
