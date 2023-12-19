using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
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

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
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

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var contractor = await connection.QuerySingleOrDefaultAsync<Contractor>(sql, new { id });
        return contractor is null ? throw new EntityNotFoundException(nameof(contractor)) : CacheEntity(contractor);
    }

    /// <summary>
    ///     Retrieve all <see cref="Contractor"/>.
    /// </summary>
    /// <returns>List of <see cref="Contractor"/>.</returns>
    public override async IAsyncEnumerable<Contractor> ListAllAsync(Navigation navigation)
    {
        var sql = "SELECT id, name FROM application.contractor ORDER BY id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        foreach (var item in await connection.QueryAsync<Contractor>(sql))
        {
            yield return CacheEntity(item);
        }
    }
}
