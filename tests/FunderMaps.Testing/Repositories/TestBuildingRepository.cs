using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestBuildingRepository : TestRepositoryBase<Building, string>, IBuildingRepository
    {
        public TestBuildingRepository(DataStore<Building> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public Task<bool> IsInGeoFenceAsync(Guid userId, string buildingId, CancellationToken token)
            => Task.FromResult(true);
    }
}
