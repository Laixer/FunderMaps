using Dapper;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

// TODO: Experimental, not yet in use.
internal sealed class TilesetRepository : DbServiceBase, ITilesetRepository
{
    public async Task<byte[]> GetTileAsync(string name, int zoom, int x, int y)
    {
        var sql = @"
            SELECT  tile
            FROM    maplayer.tileset AS t
            WHERE   t.id = @id
            AND     t.z = @z
            AND     t.x = @x
            AND     t.y = @y
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<byte[]>(sql, new { id = name, z = zoom, x, y })
            ?? throw new EntityNotFoundException();
    }

    public async Task AddTileAsync(string name, int zoom, int x, int y, byte[] tile)
    {
        var sql = @"
            INSERT INTO maplayer.tileset (id, z, x, y, tile)
            VALUES (@id, @z, @x, @y, @tile)";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id = name, z = zoom, x, y, tile });
    }
}
