using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

internal class RecoverySampleRepository : DbServiceBase, IRecoverySampleRepository
{
    private static void MapToWriter(DbContext context, RecoverySample entity)
    {
        context.AddParameterWithValue("recovery", entity.Recovery);
        context.AddParameterWithValue("building", entity.Building);
        context.AddParameterWithValue("note", entity.Note);
        context.AddParameterWithValue("status", entity.Status);
        context.AddParameterWithValue("type", entity.Type);
        context.AddParameterWithValue("pile_type", entity.PileType);
        context.AddParameterWithValue("contractor", entity.Contractor);
        context.AddParameterWithValue("facade", entity.Facade);
        context.AddParameterWithValue("permit", entity.Permit);
        context.AddParameterWithValue("permit_date", entity.PermitDate);
        context.AddParameterWithValue("recovery_date", entity.RecoveryDate);
    }

    private static RecoverySample MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
        => new()
        {
            Id = reader.GetInt(offset++),
            Recovery = reader.GetInt(offset++),
            Building = reader.GetString(offset++),
            CreateDate = reader.GetDateTime(offset++),
            UpdateDate = reader.GetSafeDateTime(offset++),
            DeleteDate = reader.GetSafeDateTime(offset++),
            Note = reader.GetSafeString(offset++),
            Status = reader.GetSafeStructValue<RecoveryStatus>(offset++),
            Type = reader.GetFieldValue<RecoveryType>(offset++),
            PileType = reader.GetSafeStructValue<PileType>(offset++),
            Contractor = reader.GetSafeInt(offset++),
            Facade = reader.GetSafeFieldValue<Facade[]>(offset++),
            Permit = reader.GetSafeString(offset++),
            PermitDate = reader.GetSafeDateTime(offset++),
            RecoveryDate = reader.GetSafeDateTime(offset++),
        };

    public async Task<int> AddAsync(RecoverySample entity)
    {
        var sql = @"
            INSERT INTO report.recovery_sample(
                recovery,
                building,
                note,
                status,
                type,
                pile_type,
                contractor,
                facade,
                permit,
                permit_date,
                recovery_date)
            VALUES (
                @recovery,
                @building,
                NULLIF(trim(@note), ''),
                @status,
                @type,
                @pile_type,
                @contractor,
                @facade,
                @permit,
                @permit_date,
                @recovery_date)
            RETURNING id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<int>(sql, entity);

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // MapToWriter(context, entity);

        // return await context.ScalarAsync<int>();
    }

    public async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.recovery_sample";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // return await context.ScalarAsync<long>();
    }

    public async Task<long> CountAsync(int recovery, Guid tenantId)
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.recovery_sample AS s
            JOIN    report.recovery AS r ON r.id = s.recovery
            JOIN    application.attribution AS a ON a.id = r.attribution
            WHERE   a.owner = @tenant
            AND     r.id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql, new { id = recovery, tenant = tenantId });

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", recovery);
        // context.AddParameterWithValue("tenant", tenantId);

        // return await context.ScalarAsync<long>();
    }

    public async Task DeleteAsync(int id, Guid tenantId)
    {
        var sql = @"
            DELETE
            FROM    report.recovery_sample
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", id);

        // await context.NonQueryAsync();
    }

    public async Task<RecoverySample> GetByIdAsync(int id, Guid tenantId)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    s.building_id,
                    s.create_date,
                    s.update_date,
                    s.delete_date,
                    s.note,
                    s.status,
                    s.type,
                    s.pile_type,
                    s.contractor,
                    s.facade,
                    s.permit,
                    s.permit_date,
                    s.recovery_date
            FROM    report.recovery_sample AS s
            JOIN    geocoder.building b ON b.external_id = s.building_id
            WHERE   s.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<RecoverySample>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(RecoverySample));

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", id);

        // await using var reader = await context.ReaderAsync();

        // return MapFromReader(reader);
    }

    public async IAsyncEnumerable<RecoverySample> ListAllByBuildingIdAsync(string id)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    s.building_id,
                    s.create_date,
                    s.update_date,
                    s.delete_date,
                    s.note,
                    s.status,
                    s.type,
                    s.pile_type,
                    s.contractor,
                    s.facade,
                    s.permit,
                    s.permit_date,
                    s.recovery_date
            FROM    report.recovery_sample AS s
            JOIN    geocoder.building b ON b.external_id = s.building_id
            WHERE   b.id = @building
            ORDER BY s.create_date DESC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<RecoverySample>(sql, new { building = id }))
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<RecoverySample> ListAllAsync(int recovery, Navigation navigation, Guid tenantId)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    s.building_id,
                    s.create_date,
                    s.update_date,
                    s.delete_date,
                    s.note,
                    s.status,
                    s.type,
                    s.pile_type,
                    s.contractor,
                    s.facade,
                    s.permit,
                    s.permit_date,
                    s.recovery_date
            FROM    report.recovery_sample AS s
            JOIN    geocoder.building b ON b.external_id = s.building_id
            JOIN    report.recovery AS r ON r.id = s.recovery
            JOIN    application.attribution AS a ON a.id = r.attribution
            WHERE   a.owner = @tenant
            AND     r.id = @id
            ORDER BY s.create_date DESC";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<RecoverySample>(sql, new { id = recovery, tenant = tenantId }))
        {
            yield return item;
        }
    }

    public async Task UpdateAsync(RecoverySample entity, Guid tenantId)
    {
        var sql = @"
            UPDATE  report.recovery_sample
            SET     recovery = @recovery,
                    building = @building,
                    note = NULLIF(trim(@note), ''),
                    status = @status,
                    type = @type,
                    pile_type = @pile_type,
                    contractor = @contractor,
                    facade = @facade,
                    permit = @permit,
                    permit_date = @permit_date,
                    recovery_date = @recovery_date
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, entity);
    }
}
