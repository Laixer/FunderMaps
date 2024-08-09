using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class StateRepository : DbServiceBase, IStateRepository
{
    public async Task<State> GetByExternalIdAsync(string id)
    {
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

        return await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(State));
    }

    public async Task<State> GetByExternalAddressIdAsync(string id)
    {
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

        return await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(State));
    }

    public async Task<State> GetByExternalBuildingIdAsync(string id)
    {
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

        return await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(State));
    }

    public async Task<State> GetByExternalNeighborhoodIdAsync(string id)
    {
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

        return await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(State));
    }

    public async Task<State> GetByExternalDistrictIdAsync(string id)
    {
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

        return await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(State));
    }

    public async Task<State> GetByExternalMunicipalityIdAsync(string id)
    {
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

        return await connection.QuerySingleOrDefaultAsync<State>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(State));
    }

    public async Task<State> GetByIdAsync(string id)
    {
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

        return await connection.QuerySingleOrDefaultAsync<State>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(State));
    }
}
