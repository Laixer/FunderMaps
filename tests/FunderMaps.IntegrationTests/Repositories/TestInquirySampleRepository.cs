using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class TestInquirySampleRepository : TestRepositoryBase<InquirySample, int>, IInquirySampleRepository
    {
        private static readonly Randomizer randomizer = new Randomizer();

        public TestInquirySampleRepository(EntityDataStore<InquirySample> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override ValueTask<int> AddAsync(InquirySample entity)
        {
            entity.Id = randomizer.Int(0, int.MaxValue);
            return base.AddAsync(entity);
        }

        public Task<uint> CountAsync(Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<InquirySample> GetByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<InquirySample> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<InquirySample>> ListAllAsync(Guid orgId, INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<InquirySample>> ListAllReportAsync(int report, INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<InquirySample>> ListAllReportAsync(int report, Guid orgId, INavigation navigation)
        {
            throw new NotImplementedException();
        }
    }
}
