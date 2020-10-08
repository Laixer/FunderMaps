using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class TestInquiryRepository : TestRepositoryBase<InquiryFull, int>, IInquiryRepository
    {
        private static readonly Randomizer randomizer = new Randomizer();

        public TestInquiryRepository(DataStore<InquiryFull> dataStore)
            : base(dataStore, e => e.Id)
        {
        }

        public override ValueTask<int> AddAsync(InquiryFull entity)
        {
            entity.Id = randomizer.Int(0, int.MaxValue);
            return base.AddAsync(entity);
        }

        public Task<uint> CountAsync(Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<InquiryFull> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task SetAuditStatusAsync(int id, InquiryFull entity)
        {
            throw new NotImplementedException();
        }
    }
}
