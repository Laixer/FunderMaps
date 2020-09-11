using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestBuildingRepository : IBuildingRepository
    {
        public ValueTask<string> AddAsync(Building entity) => throw new NotImplementedException();
        public ValueTask<ulong> CountAsync() => throw new NotImplementedException();
        public ValueTask DeleteAsync(string id) => throw new NotImplementedException();
        public ValueTask<Building> GetByIdAsync(string id) => throw new NotImplementedException();
        public Task<bool> IsInGeoFenceAsync(Guid userId, string buildingId, CancellationToken token) => Task.FromResult(true);
        public IAsyncEnumerable<Building> ListAllAsync(INavigation navigation) => throw new NotImplementedException();
        public ValueTask UpdateAsync(Building entity) => throw new NotImplementedException();
    }
}
