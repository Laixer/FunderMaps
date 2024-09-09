using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class SubsidenceRepository : DbServiceBase, ISubsidenceRepository
{
    public async IAsyncEnumerable<SubsidenceHistory> ListAllHistoryByIdAsync(string id)
    {
        var sql = @"
            SELECT  velocity,
                    mark_at
            FROM    data.subsidence_history
            WHERE   building_id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<SubsidenceHistory>(sql, new { id }))
        {
            yield return item;
        }
    }
}
