using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Inquiry repository.
    /// </summary>
    internal class InquiryRepository : RepositoryBase<InquiryFull, int>, IInquiryRepository
    {
        /// <summary>
        ///     Create new <see cref="InquiryFull"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="InquiryFull"/>.</returns>
        public override async Task<int> AddAsync(InquiryFull entity)
        {
            var sql = @"
                WITH attribution AS (
	                INSERT INTO application.attribution(
                        reviewer,
                        creator,
                        owner,
                        contractor)
		            VALUES (
                        @reviewer,
                        @user,
                        @tenant,
                        @contractor)
	                RETURNING id AS attribution_id
                )
                INSERT INTO report.inquiry(
	                document_name,
	                inspection,
	                joint_measurement,
	                floor_measurement,
	                note,
	                document_date,
	                document_file,
	                attribution,
                    access_policy,
	                type,
	                standard_f3o)
                SELECT @document_name,
                    @inspection,
                    @joint_measurement,
                    @floor_measurement,
                    NULLIF(trim(@note), ''),
                    @document_date,
                    @document_file,
	                attribution_id,
                    @access_policy,
	                @type,
                    @standard_f3o
                FROM attribution
                RETURNING id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("reviewer", entity.Attribution.Reviewer);
            context.AddParameterWithValue("user", AppContext.UserId);
            context.AddParameterWithValue("tenant", AppContext.TenantId);
            context.AddParameterWithValue("contractor", entity.Attribution.Contractor);

            MapToWriter(context, entity);

            return await context.ScalarAsync<int>();
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async Task<long> CountAsync()
        {
            var sql = @"
				SELECT  COUNT(*)
                FROM    report.inquiry AS i
                JOIN 	application.attribution AS a ON a.id = i.attribution
				WHERE   a.owner = @tenant";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("tenant", AppContext.TenantId);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="InquiryFull"/>.
        /// </summary>
        /// <param name="id">Entity object.</param>
        public override async Task DeleteAsync(int id)
        {
            ResetCacheEntity(id);

            var sql = @"
                DELETE
                FROM    report.inquiry AS i
                USING 	application.attribution AS a
                WHERE   a.id = i.attribution
                AND     i.id = @id
                AND     a.owner = @tenant";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await context.NonQueryAsync();
        }

        public static void MapToWriter(DbContext context, InquiryFull entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            context.AddParameterWithValue("document_name", entity.DocumentName);
            context.AddParameterWithValue("inspection", entity.Inspection);
            context.AddParameterWithValue("joint_measurement", entity.JointMeasurement);
            context.AddParameterWithValue("floor_measurement", entity.FloorMeasurement);
            context.AddParameterWithValue("note", entity.Note);
            context.AddParameterWithValue("document_date", entity.DocumentDate);
            context.AddParameterWithValue("document_file", entity.DocumentFile);
            context.AddParameterWithValue("access_policy", entity.Access.AccessPolicy);
            context.AddParameterWithValue("type", entity.Type);
            context.AddParameterWithValue("standard_f3o", entity.StandardF3o);
        }

        public static InquiryFull MapFromReader(DbDataReader reader, int offset = 0)
            => new()
            {
                Id = reader.GetInt(offset + 0),
                DocumentName = reader.GetSafeString(offset + 1),
                Inspection = reader.GetBoolean(offset + 2),
                JointMeasurement = reader.GetBoolean(offset + 3),
                FloorMeasurement = reader.GetBoolean(offset + 4),
                Note = reader.GetSafeString(offset + 5),
                DocumentDate = reader.GetDateTime(offset + 6),
                DocumentFile = reader.GetSafeString(offset + 7),
                Type = reader.GetFieldValue<InquiryType>(offset + 8),
                StandardF3o = reader.GetBoolean(offset + 9),
                Attribution = new()
                {
                    Reviewer = reader.GetFieldValue<Guid?>(offset + 10),
                    Creator = reader.GetGuid(offset + 11),
                    Owner = reader.GetGuid(offset + 12),
                    Contractor = reader.GetGuid(offset + 13),
                },
                State = new()
                {
                    AuditStatus = reader.GetFieldValue<AuditStatus>(offset + 14),
                },
                Access = new()
                {
                    AccessPolicy = reader.GetFieldValue<AccessPolicy>(offset + 15),
                },
                Record = new()
                {
                    CreateDate = reader.GetDateTime(offset + 16),
                    UpdateDate = reader.GetSafeDateTime(offset + 17),
                    DeleteDate = reader.GetSafeDateTime(offset + 18),
                },
            };

        /// <summary>
        ///     Retrieve <see cref="InquiryFull"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="InquiryFull"/>.</returns>
        public override async Task<InquiryFull> GetByIdAsync(int id)
        {
            if (TryGetEntity(id, out InquiryFull entity))
            {
                return entity;
            }

            var sql = @"
                SELECT  -- Inquiry
                        i.id,
                        i.document_name,
                        i.inspection,
                        i.joint_measurement,
                        i.floor_measurement,
                        i.note,
                        i.document_date,
                        i.document_file,
                        i.type,
                        i.standard_f3o,

                        -- Attribution
                        a.reviewer,
                        a.creator,
                        a.owner,
                        a.contractor,

                        -- State control
                        i.audit_status,

                        -- Access control
                        i.access_policy,
		                
                        -- Record control
                        i.create_date,
		                i.update_date,
		                i.delete_date
                FROM    report.inquiry AS i
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   i.id = @id
                AND     a.owner = @tenant
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Retrieve all <see cref="InquiryFull"/>.
        /// </summary>
        /// <returns>List of <see cref="InquiryFull"/>.</returns>
        public override async IAsyncEnumerable<InquiryFull> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT  -- Inquiry
                        i.id,
                        i.document_name,
                        i.inspection,
                        i.joint_measurement,
                        i.floor_measurement,
                        i.note,
                        i.document_date,
                        i.document_file,
                        i.type,
                        i.standard_f3o,

                        -- Attribution
                        a.reviewer,
                        a.creator,
                        a.owner,
                        a.contractor,

                        -- State control
                        i.audit_status,

                        -- Access control
                        i.access_policy,
		                
                        -- Record control
                        i.create_date,
		                i.update_date,
		                i.delete_date
                FROM    report.inquiry AS i
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   a.owner = @tenant";

            ConstructNavigation(sql, navigation);

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Update <see cref="InquiryFull"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async Task UpdateAsync(InquiryFull entity)
        {
            ResetCacheEntity(entity);

            var sql = @"
                    -- Attribution
                    UPDATE  application.attribution AS a
                    SET     reviewer = @reviewer,
                            contractor = @contractor
                    FROM    report.inquiry AS i
                    WHERE   a.id = i.attribution
                    AND     i.id = @id
                    AND     a.owner = @tenant;

                    -- Inquiry
                    UPDATE  report.inquiry AS i
                    SET     document_name = @document_name,
                            inspection = @inspection,
                            joint_measurement = @joint_measurement,
                            floor_measurement = @floor_measurement,
                            note = NULLIF(trim(@note), ''),
                            document_date = @document_date,
                            document_file = @document_file,
                            access_policy = @access_policy,
                            type = @type,
                            standard_f3o = @standard_f3o
                    FROM 	application.attribution AS a
                    WHERE   a.id = i.attribution
                    AND     i.id = @id
                    AND     a.owner = @tenant";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", entity.Id);
            context.AddParameterWithValue("reviewer", entity.Attribution.Reviewer);
            context.AddParameterWithValue("tenant", AppContext.TenantId);
            context.AddParameterWithValue("contractor", entity.Attribution.Contractor);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Set <see cref="InquiryFull"/> audit status.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <param name="entity">Entity object.</param>
        public async Task SetAuditStatusAsync(int id, InquiryFull entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            ResetCacheEntity(id);

            var sql = @"
                    UPDATE  report.inquiry AS i
                    SET     audit_status = @status
                    FROM 	application.attribution AS a
                    WHERE   a.id = i.attribution
                    AND     i.id = @id
                    AND     a.owner = @tenant";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);
            context.AddParameterWithValue("status", entity.State.AuditStatus);

            await context.NonQueryAsync();
        }
    }
}
