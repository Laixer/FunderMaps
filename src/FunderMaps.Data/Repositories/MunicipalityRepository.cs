using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

internal class MunicipalityRepository : RepositoryBase<Municipality, string>, IMunicipalityRepository
{
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    geocoder.municipality";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public async Task<Municipality> GetByExternalIdAsync(string id)
    {
        if (TryGetEntity(id, out Municipality? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Municipality
                    m.id,
                    m.name,
                    m.water,
                    m.external_id,
                    m.state_id
            FROM    geocoder.municipality AS m
            WHERE   m.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var municipality = await connection.QuerySingleOrDefaultAsync<Municipality>(sql, new { external_id = id });
        return municipality is null ? throw new EntityNotFoundException(nameof(Municipality)) : CacheEntity(municipality);
    }

    public async Task<Municipality> GetByExternalAddressIdAsync(string id)
    {
        if (TryGetEntity(id, out Municipality? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Municipality
                    m.id,
                    m.name,
                    m.water,
                    m.external_id,
                    m.state_id
            FROM    geocoder.address AS a
            JOIN    geocoder.address_building AS ab ON ab.address_id = a.id
            JOIN    geocoder.building_active AS ba ON ba.id = ab.building_id
            JOIN    geocoder.neighborhood AS n ON n.id = ba.neighborhood_id
            JOIN    geocoder.district d ON d.id = n.district_id
            JOIN    geocoder.municipality m ON m.id = d.municipality_id
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var municipality = await connection.QuerySingleOrDefaultAsync<Municipality>(sql, new { external_id = id });
        return municipality is null ? throw new EntityNotFoundException(nameof(Municipality)) : CacheEntity(municipality);
    }

    public async Task<Municipality> GetByExternalBuildingIdAsync(string id)
    {
        if (TryGetEntity(id, out Municipality? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Municipality
                    m.id,
                    m.name,
                    m.water,
                    m.external_id,
                    m.state_id
            FROM    geocoder.building_active AS ba
            JOIN    geocoder.neighborhood AS n on n.id = ba.neighborhood_id
            JOIN    geocoder.district d ON d.id = n.district_id
            JOIN    geocoder.municipality m ON m.id = d.municipality_id
            WHERE   ba.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var municipality = await connection.QuerySingleOrDefaultAsync<Municipality>(sql, new { external_id = id });
        return municipality is null ? throw new EntityNotFoundException(nameof(Municipality)) : CacheEntity(municipality);
    }

    public async Task<Municipality> GetByExternalNeighborhoodIdAsync(string id)
    {
        if (TryGetEntity(id, out Municipality? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Municipality
                    m.id,
                    m.name,
                    m.water,
                    m.external_id,
                    m.state_id
            FROM    geocoder.neighborhood AS n
            JOIN    geocoder.district d ON d.id = n.district_id
            JOIN    geocoder.municipality m ON m.id = d.municipality_id
            WHERE   n.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var municipality = await connection.QuerySingleOrDefaultAsync<Municipality>(sql, new { external_id = id });
        return municipality is null ? throw new EntityNotFoundException(nameof(Municipality)) : CacheEntity(municipality);
    }

    public async Task<Municipality> GetByExternalDistrictIdAsync(string id)
    {
        if (TryGetEntity(id, out Municipality? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Municipality
                    m.id,
                    m.name,
                    m.water,
                    m.external_id,
                    m.state_id
            FROM    geocoder.district d
            JOIN    geocoder.municipality m ON m.id = d.municipality_id
            WHERE   d.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var municipality = await connection.QuerySingleOrDefaultAsync<Municipality>(sql, new { external_id = id });
        return municipality is null ? throw new EntityNotFoundException(nameof(Municipality)) : CacheEntity(municipality);
    }

    public override async Task<Municipality> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out Municipality? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Municipality
                    m.id,
                    m.name,
                    m.water,
                    m.external_id,
                    m.state_id
            FROM    geocoder.municipality AS m
            WHERE   m.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var municipality = await connection.QuerySingleOrDefaultAsync<Municipality>(sql, new { id });
        return municipality is null ? throw new EntityNotFoundException(nameof(Municipality)) : CacheEntity(municipality);
    }

    public override async IAsyncEnumerable<Municipality> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- Municipality
                    m.id,
                    m.name,
                    m.water,
                    m.external_id,
                    m.state_id
            FROM    geocoder.municipality AS m
            OFFSET  @offset
            LIMIT   @limit";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Municipality>(sql, navigation))
        {
            yield return item;
        }
    }
}
