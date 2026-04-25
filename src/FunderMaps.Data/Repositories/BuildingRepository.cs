using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class BuildingRepository : DbServiceBase, IBuildingRepository
{
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

        return await connection.QuerySingleOrDefaultAsync<Building>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Building));
    }

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
            JOIN    geocoder.building_active AS ba ON ba.external_id = ab.building_id
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Building>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Building));
    }

    // TODO: Maybe move this to incident repository?
    public async Task<Building> GetByIncidentIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Building
                    ba.id,
                    ba.built_year,
                    ba.external_id,
                    ba.neighborhood_id
            FROM    report.incident i
            JOIN    geocoder.building_active AS ba ON ba.external_id = i.building_id
            WHERE   i.id = upper(@id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Building>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Building));
    }

    public async Task<Building> GetByIdAsync(string id)
    {
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

        return await connection.QuerySingleOrDefaultAsync<Building>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Building));
    }
}
