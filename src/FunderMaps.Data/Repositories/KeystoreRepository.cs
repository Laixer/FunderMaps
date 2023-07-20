using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     KeyStore repository.
/// </summary>
internal class KeystoreRepository : RepositoryBase<KeyStore, string>, IKeystoreRepository
{
    public override async Task<string> AddAsync(KeyStore entity)
    {
        var sql = @"
            INSERT INTO application.key_store(name, value)
            VALUES (@name, @value)
            RETURNING name";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("name", entity.Name);
        context.AddParameterWithValue("value", entity.Value);

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
            FROM    application.key_store";

        await using var context = await DbContextFactory.CreateAsync(sql);

        return await context.ScalarAsync<long>();
    }

    private static KeyStore MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Name = reader.GetString(offset++),
            Value = reader.GetString(offset++),
        };

    /// <summary>
    ///     Retrieve all <see cref="KeyStore"/>.
    /// </summary>
    /// <returns>List of <see cref="KeyStore"/>.</returns>
    public override async IAsyncEnumerable<KeyStore> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- Keystore
                    ks.name,
                    ks.value
            FROM    application.key_store ks";

        await using var context = await DbContextFactory.CreateAsync(sql);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }
}
