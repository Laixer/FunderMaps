using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class SubsidenceRepository : DbServiceBase, ISubsidenceRepository
{
    public async IAsyncEnumerable<BuildingSubsidenceHistory> ListAllHistoryByIdAsync(string id)
    {
        var sql = @"
            SELECT  velocity,
                    mark_at
            FROM    data.building_subsidence_history
            WHERE   building_id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<BuildingSubsidenceHistory>(sql, new { id }))
        {
            yield return item;
        }
    }
}
