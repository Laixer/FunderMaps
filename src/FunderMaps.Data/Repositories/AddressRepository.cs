using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class AddressRepository : DbServiceBase, IAddressRepository
{
    public async Task<Address> GetByIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Address
                    a.id,
                    a.building_number,
                    a.postal_code,
                    a.street,
                    a.external_id,
                    a.city,
                    a.building_id
            FROM    geocoder.address AS a
            WHERE   a.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Address>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Address));
    }

    public async Task<Address> GetByExternalIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Address
                    a.id,
                    a.building_number,
                    a.postal_code,
                    a.street,
                    a.external_id,
                    a.city,
                    a.building_id
            FROM    geocoder.address AS a
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Address>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Address));
    }

    public async Task<Address> GetByExternalBuildingIdAsync(string id)
    {
        var sql = @"
            SELECT  -- Address
                    a.id,
                    a.building_number,
                    a.postal_code,
                    a.street,
                    a.external_id,
                    a.city,
                    a.building_id
            FROM    geocoder.address AS a
            JOIN    geocoder.address_building AS ab ON ab.address_id = a.id
            JOIN    geocoder.building_active AS ba ON ba.external_id = ab.building_id
            WHERE   ba.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Address>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Address));
    }
}
