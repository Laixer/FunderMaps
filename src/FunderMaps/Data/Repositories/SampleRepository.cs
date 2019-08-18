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
                    .ThenInclude(si => si.Attribution)
                .Include(s => s.Address);
        }

        public override Task<Sample> GetByIdAsync(int id)
        {
            return DefaultQuery().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IReadOnlyList<Sample>> ListAllAsync(Navigation navigation)
        {
            return await DefaultQuery()
                .OrderByDescending(s => s.CreateDate)
                .Skip(navigation.Offset)
                .Take(navigation.Limit)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Sample>> ListAllAsync(int org_id, Navigation navigation)
        {
            return await DefaultQuery()
                .Where(s => s.ReportNavigation.Attribution._Owner == org_id || s.AccessPolicy == AccessPolicy.Public)
                .OrderByDescending(s => s.CreateDate)
                .Skip(navigation.Offset)
                .Take(navigation.Limit)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Sample>> ListAllReportAsync(int report, Navigation navigation)
        {
            return await DefaultQuery()
                .Where(s => s.ReportNavigation.Id == report)
                .OrderByDescending(s => s.CreateDate)
                .Skip(navigation.Offset)
                .Take(navigation.Limit)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Sample>> ListAllReportAsync(int report, int org_id, Navigation navigation)
        {
            return await DefaultQuery()
                .Where(s => s.ReportNavigation.Attribution._Owner == org_id || s.AccessPolicy == AccessPolicy.Public)
                .Where(s => s.ReportNavigation.Id == report)
                .OrderByDescending(s => s.CreateDate)
                .Skip(navigation.Offset)
                .Take(navigation.Limit)
                .ToListAsync();
        }

        public async Task<int> CountAsync(int org_id, IDbConnection connection = null)
        {
            var sql = @"SELECT COUNT(*)
                        FROM   report.sample AS samp
                                INNER JOIN report.address AS addr ON samp.address = addr.id
                                INNER JOIN report.report AS reprt ON samp.report = reprt.id
                                INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL
                                AND (attr.owner = @Owner
                                        OR samp.access_policy = 'public')";

            return await RunSqlCommand(async cnn =>
                await cnn.QuerySingleAsync<int>(sql, new { Owner = org_id }),
                connection);
        }
    }
}
