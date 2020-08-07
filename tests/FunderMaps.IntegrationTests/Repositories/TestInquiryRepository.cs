using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class TestInquiryRepository : TestRepositoryBase<Inquiry, int>, IInquiryRepository
    {
        private static readonly Randomizer randomizer = new Randomizer();

        public TestInquiryRepository(EntityDataStore<Inquiry> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override ValueTask<int> AddAsync(Inquiry entity)
        {
            entity.Id = randomizer.Int(0, int.MaxValue);
            return base.AddAsync(entity);
        }

        public Task<uint> CountAsync(Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<Inquiry> GetByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<Inquiry> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Inquiry>> ListAllAsync(Guid orgId, INavigation navigation)
        {
            throw new NotImplementedException();
        }
    }
}
