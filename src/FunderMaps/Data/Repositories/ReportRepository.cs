using Dapper;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Report repository.
    /// </summary>
    public class ReportRepository : RepositoryBase<Report2, int>, IReportRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public ReportRepository(DbProvider dbProvider)
            : base(dbProvider)
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
        public override async Task<IReadOnlyList<Report2>> ListAllAsync(Navigation navigation)
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
        /// Create new report.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity.</returns>
        public override async Task<Report2> AddAsync(Report2 entity)
        {
            // TODO: Add attribution
            // NOTE: The SQL casts the enums because Dapper.ITypeHandler is broken
            var sql = @"
                INSERT INTO report.report
                        (document_id,
                            inspection,
                            joint_measurement,
                            floor_measurement,
                            note,
                            status,
                            type,
                            document_date,
                            document_name,
                            access_policy)
                VALUES      (@DocumentId,
                            @Inspection,
                            @JointMeasurement,
                            @FloorMeasurement,
                            @Note,
                            @Status,
                            @Type,
                            @DocumentDate,
                            @DocumentName,
                            @EnforcementTerm
                            (enum_range(NULL::attestation.access_policy_type))[@AccessPolicy + 1])
                RETURNING id";

            var id = await RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<int>(sql, entity));
            return await GetByIdAsync(id);
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public override Task UpdateAsync(Report2 entity)
        {
            // TODO: Add address, foundation_type, foundation_damage_cause
            // NOTE: The SQL casts the enums because Dapper.ITypeHandler is broken
            var sql = @"
                UPDATE report.report AS reprt
                SET    inspection = @Inspection,
                        joint_measurement = @JointMeasurement,
                        floor_measurement = @FloorMeasurement,
                        note = @Note,
                        status = @Status,
                        type = @Type
                        document_date = @DocumentDate,
                        document_name = @DocumentName,
                        access_policy = (enum_range(NULL::attestation.access_policy_type))[@AccessPolicy + 1]
                WHERE  reprt.delete_date IS NULL
                        AND reprt.id = @Id";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Update report status.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <param name="status">New status.</param>
        public Task UpdateStatusAsync(Report2 entity, ReportStatus status)
        {
            var sql = @"
                UPDATE report.report AS reprt 
                SET    status = (enum_range(NULL::report.report_status))[@Status + 1]
                WHERE  reprt.id = @Id ";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, new { Status = status, entity.Id }));
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(Report2 entity)
        {
            var sql = @"UPDATE report.report AS reprt
                        SET    delete_date = CURRENT_TIMESTAMP
                        WHERE  reprt.delete_date IS NULL
                               AND reprt.id = @Id";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Count entities and filter on access policy and organization.
        /// </summary>
        /// <param name="org_id">Organization identifier.</param>
        /// <returns>Number of records.</returns>
        public Task<uint> CountAsync(int org_id)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM   report.report AS reprt
                        INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL
                        AND (attr.owner = @Owner
                                OR reprt.access_policy = 'public')";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql, new { Owner = org_id }));
        }

        /// <summary>
        /// Count entities.
        /// </summary>
        /// <returns>Number of records.</returns>
        public override Task<uint> CountAsync()
        {
            var sql = @"
                SELECT COUNT(*)
                FROM   report.report AS reprt
                        INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql));
        }
    }
}
