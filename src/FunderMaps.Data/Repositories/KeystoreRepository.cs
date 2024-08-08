using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

internal class KeystoreRepository : RepositoryBase<KeyStore, string>, IKeystoreRepository
{
    public async Task<string> AddAsync(KeyStore entity)
    {
        var sql = @"
            INSERT INTO application.key_store(name, value)
            VALUES (@name, @value)
            RETURNING name";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<string>(sql, entity) ?? throw new DatabaseException("Unable to insert record.");
    }

    public async IAsyncEnumerable<KeyStore> ListAllAsync()
    {
        var sql = @"
            SELECT  -- Keystore
                    ks.name,
                    ks.value
            FROM    application.key_store ks";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<KeyStore>(sql))
        {
            yield return item;
        }
    }
}
