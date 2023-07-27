using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Address repository.
/// </summary>
internal class AddressRepository : RepositoryBase<Address, string>, IAddressRepository
{
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    geocoder.address";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public async Task<Address> GetByExternalIdAsync(string id)
    {
        if (TryGetEntity(id, out Address? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Address
                    a.id,
                    a.building_number,
                    a.postal_code,
                    a.street,
                    a.is_active,
                    a.external_id,
                    a.city,
                    a.building_id
            FROM    geocoder.address AS a
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var address = await connection.QuerySingleOrDefaultAsync<Address>(sql, new { external_id = id });
        if (address is null)
        {
            throw new EntityNotFoundException(nameof(Address));
        }

        return CacheEntity(address);
    }

    /// <summary>
    ///     Retrieve <see cref="Address"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Address"/>.</returns>
    public override async Task<Address> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out Address? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  -- Address
                    a.id,
                    a.building_number,
                    a.postal_code,
                    a.street,
                    a.is_active,
                    a.external_id,
                    a.city,
                    a.building_id
            FROM    geocoder.address AS a
            WHERE   a.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var address = await connection.QuerySingleOrDefaultAsync<Address>(sql, new { id });
        if (address is null)
        {
            throw new EntityNotFoundException(nameof(Address));
        }

        return CacheEntity(address);
    }

    /// <summary>
    ///     Retrieve all <see cref="Address"/>.
    /// </summary>
    /// <returns>List of <see cref="Address"/>.</returns>
    public override async IAsyncEnumerable<Address> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- Address
                    a.id,
                    a.building_number,
                    a.postal_code,
                    a.street,
                    a.is_active,
                    a.external_id,
                    a.city,
                    a.building_id
            FROM    geocoder.address AS a
            OFFSET  @offset
            LIMIT   @limit";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        foreach (var item in await connection.QueryAsync<Address>(sql, navigation))
        {
            yield return CacheEntity(item);
        }
    }
}
