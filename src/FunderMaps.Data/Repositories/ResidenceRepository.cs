using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

internal class ResidenceRepository : RepositoryBase<Residence, string>, IResidenceRepository
{
    public async Task<Residence> GetByExternalBuildingIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Residence
                    r.id,
                    r.address_id,
                    r.building_id,
                    ST_X(r.geom) AS longitude, 
                    ST_Y(r.geom) AS latitude
            FROM    geocoder.residence AS r
            WHERE   r.building_id = upper(@building_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var residence = await connection.QuerySingleOrDefaultAsync<Residence>(sql, new { building_id = id });
        return residence is null ? throw new EntityNotFoundException(nameof(Residence)) : CacheEntity(residence);
    }

    public async Task<Residence> GetByExternalAddressIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Residence
                    r.id,
                    r.address_id,
                    r.building_id,
                    ST_X(r.geom) AS longitude, 
                    ST_Y(r.geom) AS latitude
            FROM    geocoder.residence AS r
            WHERE   r.address_id = upper(@address_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var residence = await connection.QuerySingleOrDefaultAsync<Residence>(sql, new { address_id = id });
        return residence is null ? throw new EntityNotFoundException(nameof(Residence)) : CacheEntity(residence);
    }

    public override async Task<Residence> GetByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Residence
                    r.id,
                    r.address_id,
                    r.building_id,
                    ST_X(r.geom) AS longitude, 
                    ST_Y(r.geom) AS latitude
            FROM    geocoder.residence AS r
            WHERE   r.id = upper(@id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var residence = await connection.QuerySingleOrDefaultAsync<Residence>(sql, new { id });
        return residence is null ? throw new EntityNotFoundException(nameof(Residence)) : CacheEntity(residence);
    }
}
