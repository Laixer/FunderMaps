using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Contractor repository.
/// </summary>
internal class ContractorRepository : RepositoryBase<Contractor, int>, IContractorRepository
{
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = "SELECT count(*) FROM application.contractor";

        var conn = DbContextFactory.DbProvider.ConnectionScope();

        return await conn.ExecuteScalarAsync<long>(sql);
    }

    /// <summary>
    ///     Retrieve <see cref="Contractor"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Contractor"/>.</returns>
    public override async Task<Contractor> GetByIdAsync(int id)
    {
        if (TryGetEntity(id, out Contractor? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = "SELECT id, name FROM application.contractor WHERE id = @id ORDER BY id";

        var conn = DbContextFactory.DbProvider.ConnectionScope();

        return CacheEntity(await conn.QuerySingleOrDefaultAsync<Contractor>(sql, new { id }));
    }

    /// <summary>
    ///     Retrieve all <see cref="Contractor"/>.
    /// </summary>
    /// <returns>List of <see cref="Contractor"/>.</returns>
    public override async IAsyncEnumerable<Contractor> ListAllAsync(Navigation navigation)
    {
        var sql = "SELECT id, name FROM application.contractor ORDER BY id";

        var conn = DbContextFactory.DbProvider.ConnectionScope();

        foreach (var item in await conn.QueryAsync<Contractor>(sql))
        {
            yield return CacheEntity(item);
        }
    }
}
