using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     District repository.
/// </summary>
internal class DistrictRepository : RepositoryBase<District, string>, IDistrictRepository
{
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    geocoder.district";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public async Task<District> GetByExternalIdAsync(string id)
    {
        if (TryGetEntity(id, out District? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var district = await connection.QuerySingleOrDefaultAsync<District>(sql, new { external_id = id });
        return district is null ? throw new EntityNotFoundException(nameof(District)) : CacheEntity(district);
    }

    public async Task<District> GetByExternalAddressIdAsync(string id)
    {
        if (TryGetEntity(id, out District? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var district = await connection.QuerySingleOrDefaultAsync<District>(sql, new { external_id = id });
        return district is null ? throw new EntityNotFoundException(nameof(District)) : CacheEntity(district);
    }

    public async Task<District> GetByExternalBuildingIdAsync(string id)
    {
        if (TryGetEntity(id, out District? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var district = await connection.QuerySingleOrDefaultAsync<District>(sql, new { external_id = id });
        return district is null ? throw new EntityNotFoundException(nameof(District)) : CacheEntity(district);
    }

    public async Task<District> GetByExternalNeighborhoodIdAsync(string id)
    {
        if (TryGetEntity(id, out District? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var district = await connection.QuerySingleOrDefaultAsync<District>(sql, new { external_id = id });
        return district is null ? throw new EntityNotFoundException(nameof(District)) : CacheEntity(district);
    }


    /// <summary>
    ///     Retrieve <see cref="District"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="District"/>.</returns>
    public override async Task<District> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out District? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

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

        var district = await connection.QuerySingleOrDefaultAsync<District>(sql, new { id });
        return district is null ? throw new EntityNotFoundException(nameof(District)) : CacheEntity(district);
    }

    /// <summary>
    ///     Retrieve all <see cref="District"/>.
    /// </summary>
    /// <returns>List of <see cref="District"/>.</returns>
    public override async IAsyncEnumerable<District> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- District
                    d.id,
                    d.name,
                    d.water,
                    d.external_id,
                    d.municipality_id
            FROM    geocoder.district AS d
            OFFSET  @offset
            LIMIT   @limit";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<District>(sql, navigation))
        {
            yield return CacheEntity(item);
        }
    }
}
