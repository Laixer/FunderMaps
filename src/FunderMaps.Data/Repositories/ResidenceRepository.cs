using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Residence repository.
/// </summary>
internal class ResidenceRepository : RepositoryBase<Residence, string>, IResidenceRepository
{
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    geocoder.residence";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public async Task<Residence> GetByExternalBuildingIdAsync(string id)
    {
        if (TryGetEntity(id, out Residence? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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
        if (TryGetEntity(id, out Residence? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

    /// <summary>
    ///     Retrieve <see cref="Residence"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Residence"/>.</returns>
    public override async Task<Residence> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out Residence? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

    /// <summary>
    ///     Retrieve all <see cref="Residence"/>.
    /// </summary>
    /// <returns>List of <see cref="Residence"/>.</returns>
    public override async IAsyncEnumerable<Residence> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- Residence
                    r.id,
                    r.address_id,
                    r.building_id,
                    ST_X(r.geom) AS longitude, 
                    ST_Y(r.geom) AS latitude
            FROM    geocoder.residence AS r
            OFFSET  @offset
            LIMIT   @limit";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Residence>(sql, navigation))
        {
            yield return CacheEntity(item);
        }
    }
}
