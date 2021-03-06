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
    ///     Recovery repository.
    /// </summary>
    internal class RecoveryRepository : RepositoryBase<Recovery, int>, IRecoveryRepository
    {
        public static void MapToWriter(DbContext context, Recovery entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            context.AddParameterWithValue("note", entity.Note);
            context.AddParameterWithValue("access_policy", entity.Access.AccessPolicy);
            context.AddParameterWithValue("type", entity.Type);
            context.AddParameterWithValue("document_date", entity.DocumentDate);
            context.AddParameterWithValue("document_file", entity.DocumentFile);
            context.AddParameterWithValue("document_name", entity.DocumentName);
        }

        public static Recovery MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new()
            {
                Id = reader.GetInt(offset + 0),
                Note = reader.GetSafeString(offset + 1),
                Type = reader.GetFieldValue<RecoveryDocumentType>(offset + 2),
                DocumentDate = reader.GetDateTime(offset + 3),
                DocumentFile = reader.GetSafeString(offset + 4),
                DocumentName = reader.GetSafeString(offset + 5),
                Attribution = new()
                {
                    Reviewer = reader.GetFieldValue<Guid?>(offset + 6),
                    Creator = reader.GetGuid(offset + 7),
                    Owner = reader.GetGuid(offset + 8),
                    Contractor = reader.GetGuid(offset +9),
                },
                State = new()
                {
                    AuditStatus = reader.GetFieldValue<AuditStatus>(offset + 10),
                },
                Access = new()
                {
                    AccessPolicy = reader.GetFieldValue<AccessPolicy>(offset + 11),
                },
                Record = new()
                {
                    CreateDate = reader.GetDateTime(offset + 12),
                    UpdateDate = reader.GetSafeDateTime(offset + 13),
                    DeleteDate = reader.GetSafeDateTime(offset + 14),
                },
            };

        /// <summary>
        ///     Create new <see cref="Recovery"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Recovery"/>.</returns>
        public override async Task<int> AddAsync(Recovery entity)
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
                INSERT INTO report.recovery(
                    note,
                    attribution,
                    access_policy,
                    type,
                    document_date,
                    document_file,
                    document_name)
                SELECT
                    NULLIF(trim(@note), ''),
                    attribution_id,
                    @access_policy,
                    @type,
                    @document_date,
                    @document_file,
                    @document_name
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
                FROM    report.recovery";

            await using var context = await DbContextFactory.CreateAsync(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Recovery"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public override async Task DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.recovery
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Retrieve <see cref="Recovery"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Recovery"/>.</returns>
        public override async Task<Recovery> GetByIdAsync(int id)
        {
            if (TryGetEntity(id, out Recovery entity))
            {
                return entity;
            }

            var sql = @"
                SELECT  -- Recovery
                        r.id,
                        r.note,
                        r.type,
                        r.document_date,
                        r.document_file,
                        r.document_name,

                        -- Attribution
                        a.reviewer,
                        a.creator,
                        a.owner,
                        a.contractor,

                        -- State control
                        r.audit_status,

                        -- Access control
                        r.access_policy,

                        -- Record control
                        r.create_date,
		                r.update_date,
		                r.delete_date
                FROM    report.recovery AS r
                JOIN 	application.attribution AS a ON a.id = r.attribution
                WHERE   r.id = @id
                AND     a.owner = @tenant
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve all <see cref="Recovery"/>.
        /// </summary>
        /// <returns>List of <see cref="Recovery"/>.</returns>
        public override async IAsyncEnumerable<Recovery> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT  -- Recovery
                        r.id,
                        r.note,
                        r.type,
                        r.document_date,
                        r.document_file,
                        r.document_name,

                        -- Attribution
                        a.reviewer,
                        a.creator,
                        a.owner,
                        a.contractor,

                        -- State control
                        r.audit_status,

                        -- Access control
                        r.access_policy,

                        -- Record control
                        r.create_date,
		                r.update_date,
		                r.delete_date
                FROM    report.recovery AS r
                JOIN 	application.attribution AS a ON a.id = r.attribution
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
        ///     Update <see cref="Recovery"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async Task UpdateAsync(Recovery entity)
        {
            ResetCacheEntity(entity);

            var sql = @"
                    -- Attribution
                    UPDATE  application.attribution AS a
                    SET     reviewer = @reviewer,
                            contractor = @contractor
                    FROM    report.recovery AS r
                    WHERE   a.id = r.attribution
                    AND     r.id = @id
                    AND     a.owner = @tenant;
                    
                    -- Recovery
                    UPDATE  report.recovery AS r
                    SET     note = NULLIF(trim(@note), ''),
                            access_policy = @access_policy,
                            type = @type,
                            document_date = @document_date,
                            document_file = @document_file,
                            document_name = @document_name
                    FROM 	application.attribution AS a
                    WHERE   a.id = r.attribution
                    AND     r.id = @id
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
        ///     Set <see cref="Recovery"/> audit status.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <param name="entity">Entity object.</param>
        public async Task SetAuditStatusAsync(int id, Recovery entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            ResetCacheEntity(id);

            var sql = @"
                    UPDATE  report.recovery AS r
                    SET     audit_status = @status
                    FROM 	application.attribution AS a
                    WHERE   a.id = r.attribution
                    AND     r.id = @id
                    AND     a.owner = @tenant";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);
            context.AddParameterWithValue("status", entity.State.AuditStatus);

            await context.NonQueryAsync();
        }
    }
}
