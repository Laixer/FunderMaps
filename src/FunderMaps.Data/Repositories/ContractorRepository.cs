using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Contractor repository.
/// </summary>
internal class ContractorRepository : RepositoryBase<Contractor, int>, IContractorRepository
{
    public override Task<int> AddAsync(Contractor entity)
        => throw new InvalidOperationException();

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    application.contractor";

        await using var context = await DbContextFactory.CreateAsync(sql);

        return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Delete <see cref="Contractor"/>.
    /// </summary>
    /// <param name="id">Entity id.</param>
    public override async Task DeleteAsync(int id)
    {
        ResetCacheEntity(id);

        var sql = @"
            DELETE
            FROM    application.contractor
            WHERE   id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await context.NonQueryAsync();
    }

    private static Contractor MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Id = reader.GetInt(offset++),
            Name = reader.GetSafeString(offset++),
        };

    /// <summary>
    ///     Retrieve <see cref="Contractor"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Contractor"/>.</returns>
    public override async Task<Contractor> GetByIdAsync(int id)
    {
        if (TryGetEntity(id, out Contractor entity))
        {
            return entity;
        }

        var sql = @"
            SELECT  id,
                    name
            FROM    application.contractor
            WHERE   id = @id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await using var reader = await context.ReaderAsync();

        return CacheEntity(MapFromReader(reader));
    }

    /// <summary>
    ///     Retrieve all <see cref="Contractor"/>.
    /// </summary>
    /// <returns>List of <see cref="Contractor"/>.</returns>
    public override async IAsyncEnumerable<Contractor> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  id,
                    name
            FROM    application.contractor";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }

    /// <summary>
    ///     Update <see cref="Contractor"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    public override Task UpdateAsync(Contractor entity)
        => throw new InvalidOperationException();
}
