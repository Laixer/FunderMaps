using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Recovery repository.
/// </summary>
internal class RecoveryRepository : RepositoryBase<Recovery, int>, IRecoveryRepository
{
    private static void MapToWriter(DbContext context, Recovery entity)
    {
        context.AddParameterWithValue("note", entity.Note);
        context.AddParameterWithValue("access_policy", entity.Access.AccessPolicy);
        context.AddParameterWithValue("type", entity.Type);
        context.AddParameterWithValue("document_date", entity.DocumentDate);
        context.AddParameterWithValue("document_file", entity.DocumentFile);
        context.AddParameterWithValue("document_name", entity.DocumentName);
    }

    // public static Recovery MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
    //     => new()
    //     {
    //         Id = reader.GetInt(offset + 0),
    //         Note = reader.GetSafeString(offset + 1),
    //         Type = reader.GetFieldValue<RecoveryDocumentType>(offset + 2),
    //         DocumentDate = reader.GetDateTime(offset + 3),
    //         DocumentFile = reader.GetString(offset + 4),
    //         DocumentName = reader.GetString(offset + 5),
    //         Attribution = new()
    //         {
    //             Reviewer = reader.GetFieldValue<Guid>(offset + 6),
    //             Creator = reader.GetGuid(offset + 7),
    //             Owner = reader.GetGuid(offset + 8),
    //             Contractor = reader.GetInt(offset + 9),
    //         },
    //         State = new()
    //         {
    //             AuditStatus = reader.GetFieldValue<AuditStatus>(offset + 10),
    //         },
    //         Access = new()
    //         {
    //             AccessPolicy = reader.GetFieldValue<AccessPolicy>(offset + 11),
    //         },
    //         Record = new()
    //         {
    //             CreateDate = reader.GetDateTime(offset + 12),
    //             UpdateDate = reader.GetSafeDateTime(offset + 13),
    //             DeleteDate = reader.GetSafeDateTime(offset + 14),
    //         },
    //     };

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
                    contractor,
                    contractor2)
                VALUES (
                    @reviewer,
                    @user,
                    @tenant,
                    'd8c19418-c832-4c91-8993-84b8ed641448',
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
        context.AddParameterWithValue("user", entity.Attribution.Creator);
        context.AddParameterWithValue("tenant", entity.Attribution.Owner);
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

    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Delete <see cref="Recovery"/>.
    /// </summary>
    /// <param name="id">Entity id.</param>
    /// <param name="tenantId">Tenant id.</param>
    public async Task DeleteAsync(int id, Guid tenantId)
    {
        ResetCacheEntity(id);

        var sql = @"
            DELETE
            FROM    report.recovery AS r
            USING 	application.attribution AS a
            WHERE   a.id = r.attribution
            AND     i.id = @id
            AND     a.owner = @tenant";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("tenant", tenantId);

        await context.NonQueryAsync();
    }

    public override Task<Recovery> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Retrieve <see cref="Recovery"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Recovery"/>.</returns>
    public async Task<Recovery> GetByIdAsync(int id, Guid tenantId)
    {
        if (TryGetEntity(id, out Recovery? entity))
        {
            return entity ?? throw new InvalidOperationException();
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
                    u.email AS reviewer_name, 
                    a.creator,
                    u2.email AS creator_name,
                    a.owner,
                    o.name AS owner_name,
                    a.contractor,
                    c.name AS contractor_name,

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
            JOIN    application.user u ON u.id = a.reviewer
            JOIN    application.user u2 ON u2.id = a.creator
            JOIN    application.organization o ON o.id = a.owner
            JOIN    application.contractor c ON c.id = a.contractor
            WHERE   r.id = @id
            AND     a.owner = @tenant
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var recovery = await connection.QuerySingleOrDefaultAsync<Recovery>(sql, new { id, tenant = tenantId });
        return recovery is null ? throw new EntityNotFoundException(nameof(Recovery)) : CacheEntity(recovery);
    }

    public override IAsyncEnumerable<Recovery> ListAllAsync(Navigation navigation)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Retrieve all <see cref="Recovery"/>.
    /// </summary>
    /// <returns>List of <see cref="Recovery"/>.</returns>
    public async IAsyncEnumerable<Recovery> ListAllAsync(Navigation navigation, Guid tenantId)
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
                    u.email AS reviewer_name, 
                    a.creator,
                    u2.email AS creator_name,
                    a.owner,
                    o.name AS owner_name,
                    a.contractor,
                    c.name AS contractor_name,

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
            JOIN    application.user u ON u.id = a.reviewer
            JOIN    application.user u2 ON u2.id = a.creator
            JOIN    application.organization o ON o.id = a.owner
            JOIN    application.contractor c ON c.id = a.contractor
            WHERE   a.owner = @tenant
            ORDER BY coalesce(r.update_date, r.create_date) DESC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Recovery>(sql, new { tenant = tenantId }))
        {
            yield return CacheEntity(item);
        }
    }

    public async IAsyncEnumerable<Recovery> ListAllByBuildingIdAsync(Navigation navigation, Guid tenantId, string id)
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
                    u.email AS reviewer_name, 
                    a.creator,
                    u2.email AS creator_name,
                    a.owner,
                    o.name AS owner_name,
                    a.contractor,
                    c.name AS contractor_name,

                    -- State control
                    r.audit_status,

                    -- Access control
                    r.access_policy,

                    -- Record control
                    r.create_date,
                    r.update_date,
                    r.delete_date
            FROM    report.recovery_sample AS s
            JOIN    report.recovery AS r ON r.id = s.recovery
            JOIN    application.attribution AS a ON a.id = r.attribution
            JOIN    application.user u ON u.id = a.reviewer
            JOIN    application.user u2 ON u2.id = a.creator
            JOIN    application.organization o ON o.id = a.owner
            JOIN    application.contractor c ON c.id = a.contractor
            WHERE   s.building = @building
            GROUP BY r.id, a.reviewer, u.email, a.creator, u2.email, a.owner, o.name, a.contractor, c.name
            ORDER BY coalesce(r.update_date, r.create_date) DESC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Recovery>(sql, new { building = id }))
        {
            yield return CacheEntity(item);
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
        context.AddParameterWithValue("tenant", entity.Attribution.Owner);
        context.AddParameterWithValue("contractor", entity.Attribution.Contractor);

        MapToWriter(context, entity);

        await context.NonQueryAsync();
    }

    /// <summary>
    ///     Set <see cref="Recovery"/> audit status.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="entity">Entity object.</param>
    public async Task SetAuditStatusAsync(int id, Recovery entity, Guid tenantId)
    {
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
        context.AddParameterWithValue("tenant", tenantId);
        context.AddParameterWithValue("status", entity.State.AuditStatus);

        await context.NonQueryAsync();
    }
}
