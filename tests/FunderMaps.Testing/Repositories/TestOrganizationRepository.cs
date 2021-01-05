using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestOrganizationRepository : TestRepositoryBase<Organization, Guid>, IOrganizationRepository
    {
        /// <summary>
        ///     Datastore holding the organization proposal entities.
        /// </summary>
        public DataStore<OrganizationProposal> DataStoreOrganizationProposal { get; set; }

        public TestOrganizationRepository(DataStore<Organization> dataStore, DataStore<OrganizationProposal> dataStoreOrganizationProposal)
            : base(dataStore, e => e.Id)
        {
            DataStoreOrganizationProposal = dataStoreOrganizationProposal;
        }

        public override Task<Guid> AddAsync(Organization entity)
            => throw new InvalidOperationException();

        public Task<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash)
        {
            var proposal = DataStoreOrganizationProposal.ItemList.FirstOrDefault(e => e.Id == id);

            Organization organization = new()
            {
                Id = proposal.Id,
                Name = proposal.Name,
                Email = proposal.Email,
            };

            DataStore.ItemList.Add(organization);
            return Task.FromResult<Guid>(organization.Id);
        }

        public Task<Organization> GetByEmailAsync(string email)
            => Task.FromResult<Organization>(DataStore.ItemList.FirstOrDefault(e => e.Email == email));

        public Task<Organization> GetByNameAsync(string name)
            => Task.FromResult<Organization>(DataStore.ItemList.FirstOrDefault(e => e.Name == name));
    }
}
