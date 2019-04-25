using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
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
                .Include(s => s.Attribution);
        }

        public override Task<Report> GetByIdAsync(int id)
        {
            return DefaultQuery().FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
