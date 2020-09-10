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

        public override ValueTask<Guid> AddAsync(Organization entity)
            => throw new InvalidOperationException();

        public ValueTask<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash)
        {
            var proposal = DataStoreOrganizationProposal.ItemList.FirstOrDefault(e => e.Id == id);

            var organization = new Organization
            {
                Id = proposal.Id,
                Name = proposal.Name,
                Email = proposal.Email,
            };

            DataStore.ItemList.Add(organization);
            return new ValueTask<Guid>(organization.Id);
        }

        public ValueTask<Organization> GetByEmailAsync(string email)
            => new ValueTask<Organization>(DataStore.ItemList.FirstOrDefault(e => e.Email == email));

        public ValueTask<Organization> GetByNameAsync(string name)
            => new ValueTask<Organization>(DataStore.ItemList.FirstOrDefault(e => e.Name == name));
    }
}
