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
                    .ThenInclude(si => si.Status)
                .Include(s => s.Address)
                .Include(s => s.BaseMeasurementLevel)
                .Include(s => s.FoundationDamageCause)
                .Include(s => s.EnforcementTerm)
                .Include(s => s.FoundationQuality)
                .Include(s => s.FoundationType)
                .Include(s => s.Substructure)
                .Include(s => s.AccessPolicy)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
