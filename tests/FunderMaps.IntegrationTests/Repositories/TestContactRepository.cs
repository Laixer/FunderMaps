using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class TestContactRepository : TestRepositoryBase<Contact, string>, IContactRepository
    {
        public TestContactRepository(EntityDataStore<Contact> dataStore)
            : base(dataStore, e => e.Email)
        {
        }
    }
}
