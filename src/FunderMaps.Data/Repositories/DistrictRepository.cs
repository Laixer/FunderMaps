using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

internal class DistrictRepository : RepositoryBase<District, string>, IDistrictRepository
{
    public async Task<District> GetByExternalIdAsync(string id)
    {
        var sql = @"
            SELECT  -- District
                    d.id,
                    d.name,
                    d.water,
                    d.external_id,
                    d.municipality_id
            FROM    geocoder.district AS d
            WHERE   d.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<District>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(District));
    }

    public async Task<District> GetByExternalAddressIdAsync(string id)
    {
        var sql = @"
            SELECT  -- District
                    d.id,
                    d.name,
                    d.water,
                    d.external_id,
                    d.municipality_id
            FROM    geocoder.address AS a
            JOIN    geocoder.address_building AS ab ON ab.address_id = a.id
            JOIN    geocoder.building_active AS ba ON ba.id = ab.building_id
            JOIN    geocoder.neighborhood AS n ON n.id = ba.neighborhood_id
            JOIN    geocoder.district d ON d.id = n.district_id
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<District>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(District));
    }

    public async Task<District> GetByExternalBuildingIdAsync(string id)
    {
        var sql = @"
            SELECT  -- District
                    d.id,
                    d.name,
                    d.water,
                    d.external_id,
                    d.municipality_id
            FROM    geocoder.building_active AS ba
            JOIN    geocoder.neighborhood AS n on n.id = ba.neighborhood_id
            JOIN    geocoder.district d ON d.id = n.district_id
            WHERE   ba.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<District>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(District));
    }

    public async Task<District> GetByExternalNeighborhoodIdAsync(string id)
    {
        var sql = @"
            SELECT  -- District
                    d.id,
                    d.name,
                    d.water,
                    d.external_id,
                    d.municipality_id
            FROM    geocoder.neighborhood AS n
            JOIN    geocoder.district d ON d.id = n.district_id
            WHERE   n.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<District>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(District));
    }

    public async Task<District> GetByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- District
                    d.id,
                    d.name,
                    d.water,
                    d.external_id,
                    d.municipality_id
            FROM    geocoder.district AS d
            WHERE   d.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<District>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(District));
    }
}
