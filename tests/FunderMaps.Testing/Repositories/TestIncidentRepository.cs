using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
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
            entity.AuditStatus = AuditStatus.Todo;
            entity.UpdateDate = null;
            entity.DeleteDate = null;
            return base.AddAsync(entity);
        }
    }
}
