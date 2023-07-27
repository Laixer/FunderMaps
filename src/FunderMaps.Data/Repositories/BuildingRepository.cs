using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Building repository.
/// </summary>
internal class BuildingRepository : RepositoryBase<Building, string>, IBuildingRepository
{
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    geocoder.building_active";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public async Task<Building> GetByExternalIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Building
                    ba.id,
                    ba.built_year,
                    ba.external_id,
                    ba.neighborhood_id
            FROM    geocoder.building_active AS ba
            WHERE   ba.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var building = await connection.QuerySingleOrDefaultAsync<Building>(sql, new { external_id = id });
        if (building is null)
        {
            throw new EntityNotFoundException(nameof(Building));
        }

        return CacheEntity(building);
    }

    /// <summary>
    ///     Get building by external address id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single building.</returns>
    public async Task<Building> GetByExternalAddressIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Building
                    ba.id,
                    ba.built_year,
                    ba.external_id,
                    ba.neighborhood_id
            FROM    geocoder.address AS a
            JOIN    geocoder.address_building AS ab ON ab.address_id = a.id
            JOIN    geocoder.building_active AS ba ON ba.id = ab.building_id
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var building = await connection.QuerySingleOrDefaultAsync<Building>(sql, new { external_id = id });
        if (building is null)
        {
            throw new EntityNotFoundException(nameof(Building));
        }

        return CacheEntity(building);
    }

    /// <summary>
    ///     Retrieve <see cref="Building"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Building"/>.</returns>
    public override async Task<Building> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out Building? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Building
                    ba.id,
                    ba.built_year,
                    ba.external_id,
                    ba.neighborhood_id
            FROM    geocoder.building_active AS ba
            WHERE   ba.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var building = await connection.QuerySingleOrDefaultAsync<Building>(sql, new { id });
        if (building is null)
        {
            throw new EntityNotFoundException(nameof(Building));
        }

        return CacheEntity(building);
    }

    /// <summary>
    ///     Retrieve all <see cref="Building"/>.
    /// </summary>
    /// <returns>List of <see cref="Building"/>.</returns>
    public override async IAsyncEnumerable<Building> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- Building
                    ba.id,
                    ba.built_year,
                    ba.external_id,
                    ba.neighborhood_id
            FROM    geocoder.building_active AS ba
            OFFSET  @offset
            LIMIT   @limit";

        sql = ConstructNavigation(sql, navigation);

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        foreach (var item in await connection.QueryAsync<Building>(sql, navigation))
        {
            yield return CacheEntity(item);
        }
    }
}
