using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Report repository.
    /// </summary>
    public class ReportRepository : RepositoryBase<Report, int>, IReportRepository
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
        /// <returns><see cref="Sample"/> on success, null on error.</returns>
        public override async Task<Report> GetByIdAsync(int id)
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
                        FROM   application.report AS reprt
                               INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  reprt.delete_date IS NULL
                               AND reprt.id = @Id
                        LIMIT  1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report>(sql, new { Id = id }));
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
        /// <returns><see cref="Sample"/> on success, null on error.</returns>
        public async Task<Report> GetByIdAsync(int id, string document)
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
                FROM   application.report AS reprt
                        INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL
                        AND reprt.id = @Id
                        AND reprt.document_id = @DocumentId
                LIMIT  1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report>(sql, new { Id = id, DocumentId = document }));
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
        public override async Task<IReadOnlyList<Report>> ListAllAsync(Navigation navigation)
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
                FROM   application.report AS reprt
                        INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report>(sql, navigation));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return all reports and filter on access policy and organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Report>> ListAllAsync(Guid orgId, Navigation navigation)
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
                        attr.id,
                        attr.project,
						attr.reviewer,
						attr.creator,
						attr.owner,
						attr.contractor
                FROM   application.report AS reprt
                        INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL
                        AND (attr.owner = @Owner
                                OR reprt.access_policy = 'public')
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            // TODO: Move!
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessPolicy>("application.access_policy");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<ReportStatus>("application.report_status");
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<ReportType>("application.report_type");

            Report map(Report reportEntity, Attribution attributionEntity)
            {
                reportEntity.Attribution = attributionEntity;
                return reportEntity;
            };

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Report, Attribution, Report>(
                sql: sql,
                map: map,
                splitOn: "id",
                param: new { Owner = orgId, navigation.Offset, navigation.Limit }));
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
        public override async Task<Report> AddAsync(Report entity)
        {
            // TODO: Add attribution
            // NOTE: The SQL casts the enums because Dapper.ITypeHandler is broken
            var sql = @"
                INSERT INTO application.report
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
        public override Task UpdateAsync(Report entity)
        {
            // TODO: Add address, foundation_type, foundation_damage_cause
            // NOTE: The SQL casts the enums because Dapper.ITypeHandler is broken
            var sql = @"
                UPDATE application.report AS reprt
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
        public Task UpdateStatusAsync(Report entity, ReportStatus status)
        {
            var sql = @"
                UPDATE application.report AS reprt 
                SET    status = (enum_range(NULL::application.report_status))[@Status + 1]
                WHERE  reprt.id = @Id ";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, new { Status = status, entity.Id }));
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(Report entity)
        {
            var sql = @"UPDATE application.report AS reprt
                        SET    delete_date = CURRENT_TIMESTAMP
                        WHERE  reprt.delete_date IS NULL
                               AND reprt.id = @Id";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Count entities and filter on access policy and organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>Number of records.</returns>
        public Task<uint> CountAsync(Guid orgId)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM   application.report AS reprt
                        INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL
                        AND (attr.owner = @Owner
                                OR reprt.access_policy = 'public')";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql, new { Owner = orgId }));
        }

        /// <summary>
        /// Count entities.
        /// </summary>
        /// <returns>Number of records.</returns>
        public override Task<uint> CountAsync()
        {
            var sql = @"
                SELECT COUNT(*)
                FROM   application.report AS reprt
                        INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                WHERE  reprt.delete_date IS NULL";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql));
        }
    }
}
