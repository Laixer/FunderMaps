using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Neighborhood repository.
/// </summary>
internal class NeighborhoodRepository : RepositoryBase<Neighborhood, string>, INeighborhoodRepository
{
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    geocoder.neighborhood";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public async Task<Neighborhood> GetByExternalIdAsync(string id)
    {
        if (TryGetEntity(id, out Neighborhood? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var neighborhood = await connection.QuerySingleOrDefaultAsync<Neighborhood>(sql, new { external_id = id });
        return neighborhood is null ? throw new EntityNotFoundException(nameof(Neighborhood)) : CacheEntity(neighborhood);
    }

    public async Task<Neighborhood> GetByExternalAddressIdAsync(string id)
    {
        if (TryGetEntity(id, out Neighborhood? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var neighborhood = await connection.QuerySingleOrDefaultAsync<Neighborhood>(sql, new { external_id = id });
        return neighborhood is null ? throw new EntityNotFoundException(nameof(Neighborhood)) : CacheEntity(neighborhood);
    }

    public async Task<Neighborhood> GetByExternalBuildingIdAsync(string id)
    {
        if (TryGetEntity(id, out Neighborhood? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var neighborhood = await connection.QuerySingleOrDefaultAsync<Neighborhood>(sql, new { external_id = id });
        return neighborhood is null ? throw new EntityNotFoundException(nameof(Neighborhood)) : CacheEntity(neighborhood);
    }

    /// <summary>
    ///     Retrieve <see cref="Neighborhood"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Neighborhood"/>.</returns>
    public override async Task<Neighborhood> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out Neighborhood? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var neighborhood = await connection.QuerySingleOrDefaultAsync<Neighborhood>(sql, new { id });
        return neighborhood is null ? throw new EntityNotFoundException(nameof(Neighborhood)) : CacheEntity(neighborhood);
    }

    /// <summary>
    ///     Retrieve all <see cref="Neighborhood"/>.
    /// </summary>
    /// <returns>List of <see cref="Neighborhood"/>.</returns>
    public override async IAsyncEnumerable<Neighborhood> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- Neighborhood
                    n.id,
                    n.name,
                    n.external_id,
                    n.district_id
            FROM    geocoder.neighborhood AS n
            OFFSET  @offset
            LIMIT   @limit";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Neighborhood>(sql, navigation))
        {
            yield return item;
        }
    }
}
