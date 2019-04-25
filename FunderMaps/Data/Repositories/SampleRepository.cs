using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
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

        public override Task<Sample> GetByIdAsync(int id)
        {
            return _dbContext.Sample
                .Include(s => s.ReportNavigation)
                    .ThenInclude(si => si.Attribution)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<Sample> GetByIdWithItemsAsync(int id)
        {
            return _dbContext.Sample
                .Include(s => s.ReportNavigation)
                    .ThenInclude(si => si.Attribution)
                .Include(s => s.ReportNavigation)
                .Include(s => s.Address)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
