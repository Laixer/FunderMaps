using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestBuildingRepository : TestRepositoryBase<Building, string>, IBuildingRepository
    {
        public TestBuildingRepository(DataStore<Building> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public Task<bool> IsInGeoFenceAsync(Guid userId, string buildingId)
            => Task.FromResult(true);
    }
}
