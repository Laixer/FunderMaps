using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestIncidentRepository : TestRepositoryBase<Incident, string>, IIncidentRepository
    {
        private static readonly Randomizer randomizer = new Randomizer();

        public TestIncidentRepository(DataStore<Incident> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override ValueTask<string> AddAsync(Incident entity)
        {
            entity.Id = randomizer.Replace("FIR######-#####");
            return base.AddAsync(entity);
        }
    }
}
