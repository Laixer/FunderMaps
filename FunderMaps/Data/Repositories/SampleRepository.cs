using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FunderMaps.Data.Repositories
{
    public class SampleRepository : EfRepository<FisDbContext, Sample>, ISampleRepository
    {
        public SampleRepository(FisDbContext dbContext)
            : base(dbContext)
        {
        }

        private IQueryable<Sample> DefaultQuery()
        {
            return _dbContext.Sample
                .Include(s => s.ReportNavigation)
                    .ThenInclude(si => si.Attribution);
        }

        public override async Task<IReadOnlyList<Sample>> ListAllAsync()
        {
            return await DefaultQuery().ToListAsync();
        }

        public override Task<Sample> GetByIdAsync(int id)
        {
            return DefaultQuery().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IReadOnlyList<Sample>> ListAllPublicAsync(int org_id, Navigation navigation)
        {
            return await DefaultQuery()
                .Where(s => s.ReportNavigation.Attribution._Owner == org_id || s.AccessPolicy == AccessPolicy.Public)
                .OrderByDescending(s => s.CreateDate)
                .Skip(navigation.Offset)
                .Take(navigation.Limit)
                .ToListAsync();
        }

        public Task<Sample> GetByIdWithItemsAsync(int id)
        {
            return DefaultQuery()
                .Include(s => s.ReportNavigation)
                .Include(s => s.Address)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
