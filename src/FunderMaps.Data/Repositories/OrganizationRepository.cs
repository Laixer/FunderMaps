using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

internal class OrganizationRepository : RepositoryBase<Organization, Guid>, IOrganizationRepository
{
    public async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    application.organization";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    public override async Task DeleteAsync(Guid id)
    {
        Cache.Remove(id);

        var sql = @"
            DELETE
            FROM    application.organization
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    // private static Organization MapFromReader(DbDataReader reader, int offset = 0)
    //     => new()
    //     {
    //         Id = reader.GetGuid(offset++),
    //         Name = reader.GetString(offset++),
    //         Email = reader.GetString(offset++),
    //     };

    public override async Task<Organization> GetByIdAsync(Guid id)
    {
        var sql = @"
            SELECT  id,
                    name,
                    email
            FROM    application.organization
            WHERE   id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Organization>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Organization));

        // return await connection.QuerySingleOrDefaultAsync<Organization>(sql, new { id });

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", id);

        // await using var reader = await context.ReaderAsync();

        // return CacheEntity(MapFromReader(reader));
    }

    public async IAsyncEnumerable<Organization> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  id,
                    name,
                    email
            FROM    application.organization";

        // sql = ConstructNavigation(sql, navigation);

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Organization>(sql, navigation))
        {
            yield return item;
        }

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // await foreach (var reader in context.EnumerableReaderAsync())
        // {
        //     yield return CacheEntity(MapFromReader(reader));
        // }
    }

    public override async Task UpdateAsync(Organization entity)
    {
        Cache.Remove(entity.Id);

        var sql = @"
            UPDATE  application.organization
            SET     email = trim(@email)
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { entity.Id, entity.Email });
    }
}
