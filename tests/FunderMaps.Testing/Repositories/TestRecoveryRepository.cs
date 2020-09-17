using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestRecoveryRepository : TestRepositoryBase<Recovery, int>, IRecoveryRepository
    {
        private static readonly Randomizer randomizer = new Randomizer();

        public TestRecoveryRepository(DataStore<Recovery> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override ValueTask<int> AddAsync(Recovery entity)
        {
            entity.Id = randomizer.Int(0, int.MaxValue);
            return base.AddAsync(entity);
        }
    }
}
