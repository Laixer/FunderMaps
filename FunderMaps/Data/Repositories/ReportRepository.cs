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
    public class ReportRepository : EfRepository<FisDbContext, Report>, IReportRepository
    {
        public ReportRepository(FisDbContext dbContext)
            : base(dbContext)
        {
        }

        private IQueryable<Report> DefaultQuery()
        {
            return _dbContext.Report
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Reviewer)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Contractor)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Creator)
                .Include(s => s.Attribution)
                    .ThenInclude(si => si.Owner)
                .Include(s => s.Norm);
        }

        public override Task<Report> GetByIdAsync(int id)
        {
            return DefaultQuery().FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<Report> GetByIdAsync(int id, string document)
        {
            return DefaultQuery().FirstOrDefaultAsync(s => s.Id == id && s.DocumentId == document);
        }

        public async Task<IReadOnlyList<Report>> ListAllAsync(Navigation navigation)
        {
            return await DefaultQuery()
                .OrderByDescending(s => s.CreateDate)
                .Skip(navigation.Offset)
                .Take(navigation.Limit)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Report>> ListAllAsync(int org_id, Navigation navigation)
        {
            return await DefaultQuery()
                .Where(s => s.Attribution._Owner == org_id || s.AccessPolicy == AccessPolicy.Public)
                .OrderByDescending(s => s.CreateDate)
                .Skip(navigation.Offset)
                .Take(navigation.Limit)
                .ToListAsync();
        }

        public Task<int> CountAsync(int org_id)
        {
            return DefaultQuery()
                .Where(s => s.Attribution._Owner == org_id || s.AccessPolicy == AccessPolicy.Public)
                .CountAsync();
        }
    }
}
