using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestAddressRepository : TestRepositoryBase<Address, string>, IAddressRepository
    {
        public TestAddressRepository(DataStore<Address> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public Task<Address> GetByExternalIdAsync(string id, ExternalDataSource source)
        {
            return Task.FromResult<Address>(DataStore.ItemList.FirstOrDefault(e => e.ExternalId == id && e.ExternalSource == source));
        }
    }
}
