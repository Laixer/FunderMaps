using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class TestAddressRepository : IAddressRepository
    {
        public EntityDataStore<Address> DataStore { get; set; }

        public TestAddressRepository(EntityDataStore<Address> dataStore)
        {
            DataStore = dataStore;
        }

        public ValueTask<string> AddAsync(Address entity)
        {
            return new ValueTask<string>(DataStore.Add(entity).Id);
        }

        public ValueTask<ulong> CountAsync()
        {
            return new ValueTask<ulong>(DataStore.Count());
        }

        public ValueTask DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Address> GetByExternalIdAsync(string id, string source)
        {
            return new ValueTask<Address>(DataStore.Entities.FirstOrDefault(e => e.ExternalId == id && e.ExternalSource == source));
        }

        public ValueTask<Address> GetByIdAsync(string id)
        {
            return new ValueTask<Address>(DataStore.Entities.FirstOrDefault(e => e.Id == id));
        }

        public IAsyncEnumerable<Address> GetBySearchQueryAsync(string query, INavigation navigation)
        {
            var result = DataStore.Entities.Where(e =>
            {
                return e.Street.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                || e.PostalCode.Contains(query, StringComparison.InvariantCultureIgnoreCase);
            });
            return Helper.AsAsyncEnumerable(Helper.ApplyNavigation(result, navigation));
        }

        public IAsyncEnumerable<Address> ListAllAsync(INavigation navigation)
        {
            return Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.Entities, navigation));
        }

        public ValueTask UpdateAsync(Address entity)
        {
            throw new NotImplementedException();
        }
    }
}
