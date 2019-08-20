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
    public class ReportRepository : EfRepository<FisDbContext, Report2>, IReportRepository
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

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Sample2"/> on success, null on error.</returns>
        public override async Task<Report2> GetByIdAsync(int id)
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
                               AND reprt.id = @Id
                        LIMIT  1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report2>(sql, new { Id = id }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="document">Document identifier.</param>
        /// <returns><see cref="Sample2"/> on success, null on error.</returns>
        public async Task<Report2> GetByIdAsync(int id, string document)
        {
            var sql = @"
                SELECT reprt.id,
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
                        AND reprt.id = @Id
                        AND reprt.document_id = @DocumentId
                LIMIT  1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report2>(sql, new { Id = id, DocumentId = document }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Return all reports.
        /// </summary>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Report2>> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT reprt.id,
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
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report2>(sql, navigation));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return all reports and filter on access policy and organization.
        /// </summary>
        /// <param name="org_id">Organization identifier.</param>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Report2>> ListAllAsync(int org_id, Navigation navigation)
        {
            var sql = @"
                SELECT reprt.id,
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
                        AND (attr.owner = @Owner
                                OR reprt.access_policy = 'public')
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report2>(sql, new { Owner = org_id, navigation.Offset, navigation.Limit }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Update report status.
        /// </summary>
        /// <param name="id">Report id.</param>
        /// <param name="status">New status.</param>
        public Task UpdateStatusAsync(int id, ReportStatus status)
        {
            var sql = @"
                UPDATE report.report AS reprt 
                SET    status = (enum_range(NULL::report.report_status))[@Status + 1]
                WHERE  reprt.id = @Id ";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, new { Status = status, Id = id }));
        }

        /// <summary>
        /// Count entities and filter on access policy and organization.
        /// </summary>
        /// <param name="org_id">Organization identifier.</param>
        /// <returns>Number of records.</returns>
        public Task<int> CountAsync(int org_id)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM   report.report AS reprt
                        INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL
                        AND (attr.owner = @Owner
                                OR reprt.access_policy = 'public')";

            return RunSqlCommand(async cnn =>
                await cnn.QuerySingleAsync<int>(sql, new { Owner = org_id }));
        }

        /// <summary>
        /// Count entities.
        /// </summary>
        /// <returns>Number of records.</returns>
        public override Task<int> CountAsync()
        {
            var sql = @"
                SELECT COUNT(*)
                FROM   report.report AS reprt
                        INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<int>(sql));
        }
    }
}
