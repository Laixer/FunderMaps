using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using Dapper;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Report repository.
    /// </summary>
    public class ReportRepository : EfRepository<FisDbContext, Report>, IReportRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="dbProvider">Database provider.</param>
        public ReportRepository(FisDbContext dbContext, DbProvider dbProvider)
            : base(dbContext, dbProvider)
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

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Sample2"/> on success, null on error.</returns>
        public async Task<Report2> GetByIdAsync2(int id)
        {
            var sql = @"SELECT reprt.id,
                               reprt.document_id,
                               reprt.inspection,
                               reprt.joint_measurement, 
                               reprt.floor_measurement,
                               reprt.note,
                               reprt.status,
                               reprt.type,
                               reprt.document_date,
                               reprt.document_name,
                               reprt.access_policy,
                               attr.owner AS attribution
                        FROM   report.report AS reprt
                               INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  reprt.delete_date IS NULL
                               AND reprt.id = @id
                        LIMIT  1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report2>(sql, new { Id = id }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
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
