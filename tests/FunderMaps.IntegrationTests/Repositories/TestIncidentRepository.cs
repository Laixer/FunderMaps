using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
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
            entity.Id = new Randomizer().Replace("FIR######-#####");
            return base.AddAsync(entity);
        }
    }
}
