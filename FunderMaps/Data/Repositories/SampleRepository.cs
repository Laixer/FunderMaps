using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunderMaps.Core.Interfaces;
using FunderMaps.Interfaces;
using FunderMaps.Models.Fis;
using Microsoft.EntityFrameworkCore;

namespace FunderMaps.Data.Repositories
{
    public class SampleRepository : IAsyncRepository<Sample>, ISampleRepository
    {
        private readonly FisDbContext _dbContext;

        public SampleRepository(FisDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Sample> AddAsync(Sample entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Sample entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            return _dbContext.SaveChangesAsync();
        }

        public Task<Sample> GetByIdAsync(int id)
        {
            return _dbContext.Sample
                .Include(s => s.ReportNavigation)
                    .ThenInclude(si => si.Attribution)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<IReadOnlyList<Sample>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Sample entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
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
