using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace FunderMaps.Data.Repositories;

internal class AddressRepository : RepositoryBase<Address, string>, IAddressRepository
{
    public async Task<Address> GetByExternalIdAsync(string id)
    {
        if (Cache.TryGetValue(id, out Address? value))
        {
            return value ?? throw new InvalidOperationException();
        }

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

        var address = await connection.QuerySingleOrDefaultAsync<Address>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Address));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

        return Cache.Set(id, address, options);
    }

    public async Task<Address> GetByExternalBuildingIdAsync(string id)
    {
        if (Cache.TryGetValue(id, out Address? value))
        {
            return value ?? throw new InvalidOperationException();
        }

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
            JOIN    geocoder.building_active AS ba ON ba.id = ab.building_id
            WHERE   ba.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var address = await connection.QuerySingleOrDefaultAsync<Address>(sql, new { external_id = id })
            ?? throw new EntityNotFoundException(nameof(Address));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

        return Cache.Set(id, address, options);
    }

    public override async Task<Address> GetByIdAsync(string id)
    {
        if (Cache.TryGetValue(id, out Address? value))
        {
            return value ?? throw new InvalidOperationException();
        }

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

        var address = await connection.QuerySingleOrDefaultAsync<Address>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Address));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

        return Cache.Set(id, address, options);
    }
}
