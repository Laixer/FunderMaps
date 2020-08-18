using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class TestOrganizationRepository : TestRepositoryBase<Organization, Guid>, IOrganizationRepository
    {
        /// <summary>
        ///     Datastore holding the organization proposal entities.
        /// </summary>
        public EntityDataStore<OrganizationProposal> DataStoreOrganizationProposal { get; set; }

        public TestOrganizationRepository(EntityDataStore<Organization> dataStore, EntityDataStore<OrganizationProposal> dataStoreOrganizationProposal)
            : base(dataStore, e => e.Id)
        {
            DataStoreOrganizationProposal = dataStoreOrganizationProposal;
        }

        public override ValueTask<Guid> AddAsync(Organization entity)
            => throw new InvalidOperationException();

        public ValueTask<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash)
        {
            var proposal = DataStoreOrganizationProposal.Entities.FirstOrDefault(e => e.Id == id);

            var organization = new Organization
            {
                Id = proposal.Id,
                Name = proposal.Name,
                Email = proposal.Email,
            };

            DataStore.Entities.Add(organization);
            return new ValueTask<Guid>(organization.Id);
        }

        public ValueTask<Organization> GetByEmailAsync(string email)
            => new ValueTask<Organization>(DataStore.Entities.FirstOrDefault(e => e.Email == email));

        public ValueTask<Organization> GetByNameAsync(string name)
            => new ValueTask<Organization>(DataStore.Entities.FirstOrDefault(e => e.Name == name));
    }
}
