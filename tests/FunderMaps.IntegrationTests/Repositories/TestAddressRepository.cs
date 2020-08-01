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
            DataStore.Entities.Add(entity);
            return new ValueTask<string>(entity.Id);
        }

        public ValueTask<ulong> CountAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Address> GetByExternalIdAsync(string id, string source)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Address> GetByIdAsync(string id)
        {
            return new ValueTask<Address>(DataStore.Entities.FirstOrDefault(e => e.Id == id));
        }

        public IAsyncEnumerable<Address> GetBySearchQueryAsync(string query, INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Address> ListAllAsync(INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public ValueTask UpdateAsync(Address entity)
        {
            throw new NotImplementedException();
        }
    }
}
