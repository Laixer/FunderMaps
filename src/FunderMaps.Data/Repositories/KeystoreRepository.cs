using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;

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

        var conn = DbContextFactory.DbProvider.ConnectionScope();

        return await conn.ExecuteScalarAsync<string>(sql, entity);
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

        var conn = DbContextFactory.DbProvider.ConnectionScope();

        return await conn.ExecuteScalarAsync<long>(sql);
    }

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

        var conn = DbContextFactory.DbProvider.ConnectionScope();

        foreach (var item in await conn.QueryAsync<KeyStore>(sql))
        {
            yield return item;
        }
    }
}
