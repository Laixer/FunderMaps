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

        return await connection.QuerySingleOrDefaultAsync<Residence>(sql, new { building_id = id })
            ?? throw new EntityNotFoundException(nameof(Residence));
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

        return await connection.QuerySingleOrDefaultAsync<Residence>(sql, new { address_id = id })
            ?? throw new EntityNotFoundException(nameof(Residence));
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

        return await connection.QuerySingleOrDefaultAsync<Residence>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Residence));
    }
}
