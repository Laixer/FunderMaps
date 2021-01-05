using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestRecoverySampleRepository : TestRepositoryBase<RecoverySample, int>, IRecoverySampleRepository
    {
        private static readonly Randomizer randomizer = new Randomizer();

        public TestRecoverySampleRepository(DataStore<RecoverySample> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override Task<int> AddAsync(RecoverySample entity)
        {
            entity.Id = randomizer.Int(0, int.MaxValue);
            return base.AddAsync(entity);
        }
    }
}
