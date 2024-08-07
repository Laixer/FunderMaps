using System.Data.Common;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using FunderMaps.Core.Types;
using Dapper;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Inquiry repository.
/// </summary>
internal class InquiryRepository : RepositoryBase<Inquiry, int>, IInquiryRepository
{
    private static void MapToWriter(DbContext context, Inquiry entity)
    {
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

    private static Inquiry MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Id = reader.GetInt(offset + 0),
            DocumentName = reader.GetString(offset + 1),
            Inspection = reader.GetBoolean(offset + 2),
            JointMeasurement = reader.GetBoolean(offset + 3),
            FloorMeasurement = reader.GetBoolean(offset + 4),
            Note = reader.GetSafeString(offset + 5),
            DocumentDate = reader.GetDateTime(offset + 6),
            DocumentFile = reader.GetString(offset + 7),
            Type = reader.GetFieldValue<InquiryType>(offset + 8),
            StandardF3o = reader.GetBoolean(offset + 9),
            Attribution = new()
            {
                Reviewer = reader.GetFieldValue<Guid>(offset + 10),
                ReviewerName = reader.GetSafeString(offset + 11),
                Creator = reader.GetGuid(offset + 12),
                CreatorName = reader.GetSafeString(offset + 13),
                Owner = reader.GetGuid(offset + 14),
                OwnerName = reader.GetSafeString(offset + 15),
                Contractor = reader.GetInt(offset + 16),
                ContractorName = reader.GetSafeString(offset + 17),
            },
            State = new()
            {
                AuditStatus = reader.GetFieldValue<AuditStatus>(offset + 18),
            },
            Access = new()
            {
                AccessPolicy = reader.GetFieldValue<AccessPolicy>(offset + 19),
            },
            Record = new()
            {
                CreateDate = reader.GetDateTime(offset + 20),
                UpdateDate = reader.GetSafeDateTime(offset + 21),
                DeleteDate = reader.GetSafeDateTime(offset + 22),
            },
        };

    public override async Task<int> AddAsync(Inquiry entity)
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
        context.AddParameterWithValue("user", entity.Attribution.Creator);
        context.AddParameterWithValue("tenant", entity.Attribution.Owner);
        context.AddParameterWithValue("contractor", entity.Attribution.Contractor);

        MapToWriter(context, entity);

        return await context.ScalarAsync<int>();
    }

    public async Task<long> CountAsync(Guid tenantId)
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.inquiry AS i
            JOIN 	application.attribution AS a ON a.id = i.attribution
            WHERE   a.owner = @tenant";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql, new { tenant = tenantId });
    }

    public async Task DeleteAsync(int id, Guid tenantId)
    {
        ResetCacheEntity(id);

        var sql = @"
            DELETE
            FROM    report.inquiry AS i
            USING 	application.attribution AS a
            WHERE   a.id = i.attribution
            AND     i.id = @id
            AND     a.owner = @tenant";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id, tenant = tenantId });
    }

    public async Task<Inquiry> GetByIdAsync(int id, Guid tenantId)
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
                    u.email AS reviewer_name, 
                    a.creator,
                    u2.email AS creator_name,
                    a.owner,
                    o.name AS owner_name,
                    a.contractor,
                    c.name AS contractor_name,

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
            JOIN    application.user u ON u.id = a.reviewer
            JOIN    application.user u2 ON u2.id = a.creator
            JOIN    application.organization o ON o.id = a.owner
            JOIN    application.contractor c ON c.id = a.contractor
            WHERE   i.id = @id
            AND     a.owner = @tenant
            LIMIT   1";

        // TODO: Dapper can't handle multiple result sets in one go.

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // var inquiry = await connection.QuerySingleOrDefaultAsync<Inquiry>(sql, new { id, tenant = tenantId });
        // return inquiry is null ? throw new EntityNotFoundException(nameof(Inquiry)) : CacheEntity(inquiry);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", tenantId);

        await using var reader = await context.ReaderAsync();

        return MapFromReader(reader);
    }

    public async IAsyncEnumerable<Inquiry> ListAllAsync(Navigation navigation, Guid tenantId)
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
                    u.email AS reviewer_name, 
                    a.creator,
                    u2.email AS creator_name,
                    a.owner,
                    o.name AS owner_name,
                    a.contractor,
                    c.name AS contractor_name,

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
            JOIN    application.user u ON u.id = a.reviewer
            JOIN    application.user u2 ON u2.id = a.creator
            JOIN    application.organization o ON o.id = a.owner
            JOIN    application.contractor c ON c.id = a.contractor
            WHERE   a.owner = @tenant
            ORDER BY coalesce(i.update_date, i.create_date) DESC";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("tenant", tenantId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return MapFromReader(reader);
        }

        // TODO: Dapper can't handle multiple result sets in one go.

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // await foreach (var item in connection.QueryUnbufferedAsync<Inquiry>(sql, new { tenant = tenantId }))
        // {
        //     yield return CacheEntity(item);
        // }
    }

    public async IAsyncEnumerable<Inquiry> ListAllByBuildingIdAsync(Navigation navigation, Guid tenantId, string id)
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
                    u.email AS reviewer_name, 
                    a.creator,
                    u2.email AS creator_name,
                    a.owner,
                    o.name AS owner_name,
                    a.contractor,
                    c.name AS contractor_name,

                    -- State control
                    i.audit_status,

                    -- Access control
                    i.access_policy,

                    -- Record control
                    i.create_date,
                    i.update_date,
                    i.delete_date
            FROM    report.inquiry_sample AS s
            JOIN 	report.inquiry AS i ON i.id = s.inquiry
            JOIN 	application.attribution AS a ON a.id = i.attribution
            JOIN    application.user u ON u.id = a.reviewer
            JOIN    application.user u2 ON u2.id = a.creator
            JOIN    application.organization o ON o.id = a.owner
            JOIN    application.contractor c ON c.id = a.contractor
            WHERE   s.building = @building
            GROUP BY i.id, a.reviewer, u.email, a.creator, u2.email, a.owner, o.name, a.contractor, c.name
            ORDER BY coalesce(i.update_date, i.create_date) DESC";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("building", id);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return MapFromReader(reader);
        }

        // TODO: Dapper can't handle multiple result sets in one go.

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // await foreach (var item in connection.QueryUnbufferedAsync<Inquiry>(sql, new { building = id }))
        // {
        //     yield return CacheEntity(item);
        // }
    }

    public override async Task UpdateAsync(Inquiry entity)
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
        context.AddParameterWithValue("tenant", entity.Attribution.Owner); // TODO: Set via tenantId
        context.AddParameterWithValue("contractor", entity.Attribution.Contractor);

        MapToWriter(context, entity);

        await context.NonQueryAsync();
    }

    public async Task SetAuditStatusAsync(int id, Inquiry entity, Guid tenantId)
    {
        ResetCacheEntity(id);

        var sql = @"
            UPDATE  report.inquiry AS i
            SET     audit_status = @status
            FROM 	application.attribution AS a
            WHERE   a.id = i.attribution
            AND     i.id = @id
            AND     a.owner = @tenant";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id, tenant = tenantId, status = entity.State.AuditStatus });
    }
}
