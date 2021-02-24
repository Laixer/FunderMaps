using Bogus;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
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
        IAsyncEnumerable<RecoverySample> IRecoverySampleRepository.ListAllAsync(int report, Navigation navigation)
        {
            var result = DataStore.ItemList.Where(e => e.Recovery == report);
            return Helper.AsAsyncEnumerable(Helper.ApplyNavigation(result, navigation));
        }

        Task<long> IRecoverySampleRepository.CountAsync(int report)
        {
            var result = DataStore.ItemList.Where(e => e.Recovery == report);
            return Task.FromResult((long)result.Count());
        }

    }
}
