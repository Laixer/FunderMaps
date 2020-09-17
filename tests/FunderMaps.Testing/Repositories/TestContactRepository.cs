using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Testing.Repositories
{
    public class TestContactRepository : TestRepositoryBase<Contact, string>, IContactRepository
    {
        public TestContactRepository(DataStore<Contact> dataStore)
            : base(dataStore, e => e.Email)
        {
        }
    }
}
