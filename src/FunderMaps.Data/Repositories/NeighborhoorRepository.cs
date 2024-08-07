using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

internal class NeighborhoodRepository : RepositoryBase<Neighborhood, string>, INeighborhoodRepository
{
    public async Task<Neighborhood> GetByExternalIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Neighborhood
                    n.id,
                    n.name,
                    n.external_id,
                    n.district_id
            FROM    geocoder.neighborhood AS n
            WHERE   n.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Neighborhood>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Neighborhood));
    }

    public async Task<Neighborhood> GetByExternalAddressIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Neighborhood
                    n.id,
                    n.name,
                    n.external_id,
                    n.district_id
            FROM    geocoder.address AS a
            JOIN    geocoder.address_building AS ab ON ab.address_id = a.id
            JOIN    geocoder.building_active AS ba ON ba.id = ab.building_id
            JOIN    geocoder.neighborhood AS n ON n.id = ba.neighborhood_id
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Neighborhood>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Neighborhood));
    }

    public async Task<Neighborhood> GetByExternalBuildingIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Neighborhood
                    n.id,
                    n.name,
                    n.external_id,
                    n.district_id
            FROM    geocoder.building_active AS ba
            JOIN    geocoder.neighborhood AS n on n.id = ba.neighborhood_id
            WHERE   ba.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Neighborhood>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Neighborhood));
    }

    public override async Task<Neighborhood> GetByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Neighborhood
                    n.id,
                    n.name,
                    n.external_id,
                    n.district_id
            FROM    geocoder.neighborhood AS n
            WHERE   n.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Neighborhood>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Neighborhood));
    }
}
