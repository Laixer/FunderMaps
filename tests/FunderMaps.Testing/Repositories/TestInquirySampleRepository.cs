using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestInquirySampleRepository : TestRepositoryBase<InquirySample, int>, IInquirySampleRepository
    {
        private static readonly Randomizer randomizer = new Randomizer();

        public TestInquirySampleRepository(DataStore<InquirySample> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override ValueTask<int> AddAsync(InquirySample entity)
        {
            entity.Id = randomizer.Int(0, int.MaxValue);
            return base.AddAsync(entity);
        }

        public IAsyncEnumerable<InquirySample> ListAllAsync(int report, INavigation navigation)
        {
            var result = DataStore.ItemList.Where(e => e.Inquiry == report);
            return Helper.AsAsyncEnumerable(Helper.ApplyNavigation(result, navigation));
        }

        public Task<InquirySample> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }
    }
}
