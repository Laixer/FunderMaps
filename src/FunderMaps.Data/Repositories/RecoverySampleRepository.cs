using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Recovery sample repository.
/// </summary>
internal class RecoverySampleRepository : RepositoryBase<RecoverySample, int>, IRecoverySampleRepository
{
    public static void MapToWriter(DbContext context, RecoverySample entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        context.AddParameterWithValue("recovery", entity.Recovery);
        context.AddParameterWithValue("address", entity.Address);
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

    public static RecoverySample MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
        => new()
        {
            Id = reader.GetInt(offset++),
            Recovery = reader.GetInt(offset++),
            Address = reader.GetString(offset++),
            CreateDate = reader.GetDateTime(offset++),
            UpdateDate = reader.GetSafeDateTime(offset++),
            DeleteDate = reader.GetSafeDateTime(offset++),
            Note = reader.GetSafeString(offset++),
            Status = reader.GetSafe5tructValue<RecoveryStatus>(offset++),
            Type = reader.GetFieldValue<RecoveryType>(offset++),
            PileType = reader.GetSafe5tructValue<PileType>(offset++),
            Contractor = reader.GetSafeInt(offset++),
            Facade = reader.GetSafeFieldValue<Facade[]>(offset++),
            Permit = reader.GetSafeString(offset++),
            PermitDate = reader.GetSafeDateTime(offset++),
            RecoveryDate = reader.GetSafeDateTime(offset++),
        };

    /// <summary>
    ///     Create new <see cref="RecoverySample"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    /// <returns>Created <see cref="RecoverySample"/>.</returns>
    public override async Task<int> AddAsync(RecoverySample entity)
    {
        var sql = @"
            INSERT INTO report.recovery_sample(
                recovery,
                address,
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
                @address,
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        MapToWriter(context, entity);

        return await context.ScalarAsync<int>();
    }

    /// <summary>
    ///     Retrieve number of <see cref="RecoverySample"/>.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.recovery_sample";

        await using var context = await DbContextFactory.CreateAsync(sql);

        return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Retrieve number of <see cref="RecoverySample"/> for a given <see cref="Recovery"/>.
    /// </summary>
    /// <returns>Number of <see cref="RecoverySample"/>.</returns>
    public async Task<long> CountAsync(int recovery)
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    report.recovery_sample AS s
            JOIN    report.recovery AS r ON r.id = s.recovery
            JOIN    application.attribution AS a ON a.id = r.attribution
            WHERE   a.owner = @tenant
            AND     r.id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", recovery);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Delete <see cref="RecoverySample"/>.
    /// </summary>
    /// <param name="id">Entity id.</param>
    public override async Task DeleteAsync(int id)
    {
        var sql = @"
            DELETE
            FROM    report.recovery_sample
            WHERE   id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await context.NonQueryAsync();
    }

    /// <summary>
    ///     Retrieve <see cref="RecoverySample"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="RecoverySample"/>.</returns>
    public override async Task<RecoverySample> GetByIdAsync(int id)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    s.address,
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
            WHERE   id = @id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await using var reader = await context.ReaderAsync();

        return MapFromReader(reader);
    }

    public async IAsyncEnumerable<RecoverySample> ListAllByBuildingIdAsync(string id)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    s.address,
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
            JOIN    report.recovery AS r ON r.id = s.recovery
            JOIN    application.attribution AS a ON a.id = r.attribution
            WHERE   s.building = @building
            ORDER BY s.create_date DESC";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("building", id);
        // context.AddParameterWithValue("tenant", AppContext.TenantId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return MapFromReader(reader);
        }
    }

    /// <summary>
    ///     Retrieve all <see cref="RecoverySample"/>.
    /// </summary>
    /// <returns>List of <see cref="RecoverySample"/>.</returns>
    public override async IAsyncEnumerable<RecoverySample> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    s.address,
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
            JOIN    report.recovery AS r ON r.id = s.recovery
            JOIN    application.attribution AS a ON a.id = r.attribution
            WHERE   a.owner = @tenant
            ORDER BY s.create_date DESC";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return MapFromReader(reader);
        }
    }

    /// <summary>
    ///     Retrieve all <see cref="RecoverySample"/> for a given <see cref="Recovery"/>.
    /// </summary>
    /// <returns>List of <see cref="RecoverySample"/>.</returns>
    public async IAsyncEnumerable<RecoverySample> ListAllAsync(int recovery, Navigation navigation)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    s.address,
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
            JOIN    report.recovery AS r ON r.id = s.recovery
            JOIN    application.attribution AS a ON a.id = r.attribution
            WHERE   a.owner = @tenant
            AND     r.id = @id
            ORDER BY s.create_date DESC";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", recovery);
        context.AddParameterWithValue("tenant", AppContext.TenantId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }

    public override async Task UpdateAsync(RecoverySample entity)
    {
        var sql = @"
            UPDATE  report.recovery_sample
            SET     recovery = @recovery,
                    address = @address,
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", entity.Id);

        MapToWriter(context, entity);

        await context.NonQueryAsync();
    }
}
