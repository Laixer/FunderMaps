using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     State repository.
/// </summary>
internal class StateRepository : RepositoryBase<State, string>, IStateRepository
{
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    geocoder.state";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public async Task<State> GetByExternalIdAsync(string id)
    {
        if (TryGetEntity(id, out State? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- State
                    s.id,
                    s.name,
                    s.water,
                    s.external_id
            FROM    geocoder.state AS s
            WHERE   s.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var state = await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id });
        return state is null ? throw new EntityNotFoundException(nameof(State)) : CacheEntity(state);
    }

    public async Task<State> GetByExternalAddressIdAsync(string id)
    {
        if (TryGetEntity(id, out State? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- State
                    s.id,
                    s.name,
                    s.water,
                    s.external_id
            FROM    geocoder.address AS a
            JOIN    geocoder.address_building AS ab ON ab.address_id = a.id
            JOIN    geocoder.building_active AS ba ON ba.id = ab.building_id
            JOIN    geocoder.neighborhood AS n ON n.id = ba.neighborhood_id
            JOIN    geocoder.district d ON d.id = n.district_id
            JOIN    geocoder.municipality m ON m.id = d.municipality_id
            JOIN    geocoder.state s ON s.id = m.state_id
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var state = await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id });
        return state is null ? throw new EntityNotFoundException(nameof(State)) : CacheEntity(state);
    }

    public async Task<State> GetByExternalBuildingIdAsync(string id)
    {
        if (TryGetEntity(id, out State? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- State
                    s.id,
                    s.name,
                    s.water,
                    s.external_id
            FROM    geocoder.building_active AS ba
            JOIN    geocoder.neighborhood AS n on n.id = ba.neighborhood_id
            JOIN    geocoder.district d ON d.id = n.district_id
            JOIN    geocoder.municipality m ON m.id = d.municipality_id
            JOIN    geocoder.state s ON s.id = m.state_id
            WHERE   ba.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var state = await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id });
        return state is null ? throw new EntityNotFoundException(nameof(State)) : CacheEntity(state);
    }

    public async Task<State> GetByExternalNeighborhoodIdAsync(string id)
    {
        if (TryGetEntity(id, out State? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- State
                    s.id,
                    s.name,
                    s.water,
                    s.external_id
            FROM    geocoder.neighborhood AS n
            JOIN    geocoder.district d ON d.id = n.district_id
            JOIN    geocoder.municipality m ON m.id = d.municipality_id
            JOIN    geocoder.state s ON s.id = m.state_id
            WHERE   n.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var state = await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id });
        return state is null ? throw new EntityNotFoundException(nameof(State)) : CacheEntity(state);
    }

    public async Task<State> GetByExternalDistrictIdAsync(string id)
    {
        if (TryGetEntity(id, out State? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- State
                    s.id,
                    s.name,
                    s.water,
                    s.external_id
            FROM    geocoder.district d
            JOIN    geocoder.municipality m ON m.id = d.municipality_id
            JOIN    geocoder.state s ON s.id = m.state_id
            WHERE   d.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var state = await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id });
        return state is null ? throw new EntityNotFoundException(nameof(State)) : CacheEntity(state);
    }

    public async Task<State> GetByExternalMunicipalityIdAsync(string id)
    {
        if (TryGetEntity(id, out State? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- State
                    s.id,
                    s.name,
                    s.water,
                    s.external_id
            FROM    geocoder.municipality m
            JOIN    geocoder.state s ON s.id = m.state_id
            WHERE   m.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var state = await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id });
        return state is null ? throw new EntityNotFoundException(nameof(State)) : CacheEntity(state);
    }

    /// <summary>
    ///     Retrieve <see cref="State"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="State"/>.</returns>
    public override async Task<State> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out State? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- State
                    s.id,
                    s.name,
                    s.water,
                    s.external_id
            FROM    geocoder.state AS s
            WHERE   s.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var state = await connection.QuerySingleOrDefaultAsync<State>(sql, new { id });
        return state is null ? throw new EntityNotFoundException(nameof(State)) : CacheEntity(state);
    }

    /// <summary>
    ///     Retrieve all <see cref="State"/>.
    /// </summary>
    /// <returns>List of <see cref="State"/>.</returns>
    public override async IAsyncEnumerable<State> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- State
                    s.id,
                    s.name,
                    s.water,
                    s.external_id
            FROM    geocoder.state AS s
            OFFSET  @offset
            LIMIT   @limit";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<State>(sql, navigation))
        {
            yield return CacheEntity(item);
        }
    }
}
