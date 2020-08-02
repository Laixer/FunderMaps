using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class TestContactRepository : TestRepositoryBase<Contact, string>, IContactRepository
    {
        public TestContactRepository(EntityDataStore<Contact> dataStore)
            : base(dataStore, e => e.Email)
        {
        }

        public override ValueTask UpdateAsync(Contact entity)
        {
            throw new NotImplementedException();
        }
    }
}
