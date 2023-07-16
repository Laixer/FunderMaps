using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Address repository.
/// </summary>
internal class AddressRepository : RepositoryBase<Address, string>, IAddressRepository
{
    public static void MapToWriter(DbContext context, Address entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        context.AddParameterWithValue("building_number", entity.BuildingNumber);
        context.AddParameterWithValue("postal_code", entity.PostalCode);
        context.AddParameterWithValue("street", entity.Street);
        context.AddParameterWithValue("is_active", entity.IsActive);
        context.AddParameterWithValue("external_id", entity.ExternalId);
    }

    public static Address MapFromReader(DbDataReader reader, int offset = 0)
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
    ///     Create new <see cref="Address"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    /// <returns>Created <see cref="Address"/>.</returns>
    public override async Task<string> AddAsync(Address entity)
    {
        var sql = @"
            INSERT INTO geocoder.address(
                building_number,
                postal_code,
                street,
                is_active,
                external_id)
            VALUES (
                @building_number,
                @postal_code,
                @street,
                @is_active,
                upper(@external_id))
            ON CONFLICT DO NOTHING
            RETURNING id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        MapToWriter(context, entity);

        await using var reader = await context.ReaderAsync();

        return reader.GetString(0);
    }

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

    /// <summary>
    ///     Delete <see cref="Incident"/>.
    /// </summary>
    /// <param name="id">Entity id.</param>
    public override Task DeleteAsync(string id)
        => throw new InvalidOperationException();

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

    /// <summary>
    ///     Update <see cref="Address"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    public override async Task UpdateAsync(Address entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        ResetCacheEntity(entity);

        var sql = @"
            UPDATE  geocoder.address
            SET     building_number = @building_number,
                    postal_code = @postal_code,
                    street = @street,
                    is_active = @is_active,
                    external_id = upper(@external_id)
            WHERE   id = @id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", entity.Id);

        MapToWriter(context, entity);

        await context.NonQueryAsync();
    }
}
