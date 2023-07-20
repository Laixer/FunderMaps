using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Address repository.
/// </summary>
internal class AddressRepository : RepositoryBase<Address, string>, IAddressRepository
{
    private static void MapToWriter(DbContext context, Address entity)
    {
        context.AddParameterWithValue("building_number", entity.BuildingNumber);
        context.AddParameterWithValue("postal_code", entity.PostalCode);
        context.AddParameterWithValue("street", entity.Street);
        context.AddParameterWithValue("is_active", entity.IsActive);
        context.AddParameterWithValue("external_id", entity.ExternalId);
    }

    private static Address MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Id = reader.GetString(offset++),
            BuildingNumber = reader.GetString(offset++),
            PostalCode = reader.GetSafeString(offset++),
            Street = reader.GetString(offset++),
            IsActive = reader.GetBoolean(offset++),
            ExternalId = reader.GetString(offset++),
            City = reader.GetString(offset++),
            BuildingId = reader.GetSafeString(offset++),
        };

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    geocoder.address";

        await using var context = await DbContextFactory.CreateAsync(sql);

        return await context.ScalarAsync<long>();
    }

    public async Task<Address> GetByExternalIdAsync(string id)
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
            WHERE   a.external_id = upper(@external_id)
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("external_id", id);

        await using var reader = await context.ReaderAsync();

        return CacheEntity(MapFromReader(reader));
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await using var reader = await context.ReaderAsync();

        return CacheEntity(MapFromReader(reader));
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
                    a.building_id,

                    -- Building
                    b.id,
                    b.building_type,
                    b.built_year,
                    b.is_active,
                    b.external_id, 
                    b.external_source, 
                    b.geom,
                    b.neighborhood_id
            FROM    geocoder.address AS a
            JOIN    geocoder.building_encoded_geom AS b ON b.id = a.building_id";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }
}
