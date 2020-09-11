﻿using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
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

        public ValueTask<Address> GetByExternalIdAsync(string id, string source)
        {
            return new ValueTask<Address>(DataStore.ItemList.FirstOrDefault(e => e.ExternalId == id && e.ExternalSource == source));
        }

        public IAsyncEnumerable<Address> GetBySearchQueryAsync(string query, INavigation navigation)
        {
            var result = DataStore.ItemList.Where(e =>
            {
                return e.Street.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                || e.PostalCode.Contains(query, StringComparison.InvariantCultureIgnoreCase);
            });
            return Helper.AsAsyncEnumerable(Helper.ApplyNavigation(result, navigation));
        }
    }
}