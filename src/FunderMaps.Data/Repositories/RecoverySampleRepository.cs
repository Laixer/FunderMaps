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
    public async Task<long> CountAsync(int recovery, Guid tenantId)
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
        context.AddParameterWithValue("tenant", tenantId);

        return await context.ScalarAsync<long>();
    }

    public override Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Delete <see cref="RecoverySample"/>.
    /// </summary>
    /// <param name="id">Entity id.</param>
    public async Task DeleteAsync(int id, Guid tenantId)
    {
        var sql = @"
            DELETE
            FROM    report.recovery_sample
            WHERE   id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await context.NonQueryAsync();
    }

    public override Task<RecoverySample> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Retrieve <see cref="RecoverySample"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="RecoverySample"/>.</returns>
    public async Task<RecoverySample> GetByIdAsync(int id, Guid tenantId)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    b.external_id,
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
            JOIN    geocoder.building b ON b.id = s.building
            WHERE   s.id = @id
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
                    b.external_id,
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
            JOIN    geocoder.building b ON b.id = s.building
            WHERE   s.building = @building
            ORDER BY s.create_date DESC";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("building", id);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return MapFromReader(reader);
        }
    }

    public override IAsyncEnumerable<RecoverySample> ListAllAsync(Navigation navigation)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Retrieve all <see cref="RecoverySample"/>.
    /// </summary>
    /// <returns>List of <see cref="RecoverySample"/>.</returns>
    public async IAsyncEnumerable<RecoverySample> ListAllAsync(Navigation navigation, Guid tenantId)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    b.external_id,
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
            JOIN    geocoder.building b ON b.id = s.building
            WHERE   a.owner = @tenant
            ORDER BY s.create_date DESC";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("tenant", tenantId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return MapFromReader(reader);
        }
    }

    /// <summary>
    ///     Retrieve all <see cref="RecoverySample"/> for a given <see cref="Recovery"/>.
    /// </summary>
    /// <returns>List of <see cref="RecoverySample"/>.</returns>
    public async IAsyncEnumerable<RecoverySample> ListAllAsync(int recovery, Navigation navigation, Guid tenantId)
    {
        var sql = @"
            SELECT  -- RecoverySample
                    s.id,
                    s.recovery,
                    b.external_id,
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
            JOIN    geocoder.building b ON b.id = s.building
            WHERE   a.owner = @tenant
            AND     r.id = @id
            ORDER BY s.create_date DESC";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", recovery);
        context.AddParameterWithValue("tenant", tenantId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }

    public override Task UpdateAsync(RecoverySample entity)
    {
        throw new NotImplementedException();
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", entity.Id);

        MapToWriter(context, entity);

        await context.NonQueryAsync();
    }
}
