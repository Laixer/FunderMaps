using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class BundleRepository : DbServiceBase, IBundleRepository
{
    public async IAsyncEnumerable<Bundle> ListAllEnabledAsync()
    {
        var sql = $@"
            SELECT
                    b.tileset,
                    b.enabled,
                    b.map_enabled,
                    b.built_date,
                    b.precondition,
                    b.name,
                    b.zoom_min_level AS min_zoom_level,
                    b.zoom_max_level AS max_zoom_level
            FROM    maplayer.bundle AS b
            WHERE   enabled
            ORDER BY built_date ASC NULLS FIRST";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Bundle>(sql))
        {
            yield return item;
        }
    }

    public async Task<bool> RunPreconditionAsync(string id, string precondition)
    {
        var sql = $@"
            SELECT  maplayer.{precondition}(built_date) > precondition_threshold AS precondition
            FROM    maplayer.bundle
            WHERE   tileset = @tileset";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<bool>(sql, new { tileset = id });
    }

    public async Task LogBuiltTimeAsync(string id)
    {
        var sql = $@"
            UPDATE  maplayer.bundle
            SET     built_date = NOW()
            WHERE   tileset = @tileset";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { tileset = id });
    }
}
