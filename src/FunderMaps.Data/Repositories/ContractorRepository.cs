using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

internal class ContractorRepository : RepositoryBase<Contractor, int>, IContractorRepository
{
    public override async Task<long> CountAsync()
    {
        var sql = "SELECT count(*) FROM application.contractor";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public override async Task<Contractor> GetByIdAsync(int id)
    {
        var sql = "SELECT id, name FROM application.contractor WHERE id = @id ORDER BY id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Contractor>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Contractor));
    }

    public override async IAsyncEnumerable<Contractor> ListAllAsync(Navigation navigation)
    {
        var sql = "SELECT id, name FROM application.contractor ORDER BY id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Contractor>(sql))
        {
            yield return item;
        }
    }
}
