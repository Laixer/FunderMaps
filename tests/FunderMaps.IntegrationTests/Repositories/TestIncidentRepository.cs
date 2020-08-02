using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class TestIncidentRepository : TestRepositoryBase<Incident, string>, IIncidentRepository
    {
        public TestIncidentRepository(EntityDataStore<Incident> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override ValueTask<string> AddAsync(Incident entity)
        {
            entity.Id = $"FIR032020-{DataStore.Count() + 1}";
            return base.AddAsync(entity);
        }

        public override ValueTask UpdateAsync(Incident entity)
        {
            throw new NotImplementedException();
        }
    }
}
