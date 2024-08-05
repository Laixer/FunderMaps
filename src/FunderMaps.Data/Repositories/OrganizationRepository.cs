using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

// TODO: Remove Area and Center from Organization.
/// <summary>
///     Organization repository.
/// </summary>
internal class OrganizationRepository : RepositoryBase<Organization, Guid>, IOrganizationRepository
{
    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    application.organization";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    /// <summary>
    ///     Delete <see cref="Organization"/>.
    /// </summary>
    /// <param name="id">Entity id.</param>
    public override async Task DeleteAsync(Guid id)
    {
        ResetCacheEntity(id);

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

    /// <summary>
    ///     Retrieve <see cref="Organization"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Organization"/>.</returns>
    public override async Task<Organization> GetByIdAsync(Guid id)
    {
        if (TryGetEntity(id, out Organization? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  id,
                    name,
                    email
            FROM    application.organization
            WHERE   id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var organization = await connection.QuerySingleOrDefaultAsync<Organization>(sql, new { id });
        return organization is null ? throw new EntityNotFoundException(nameof(Organization)) : CacheEntity(organization);

        // return await connection.QuerySingleOrDefaultAsync<Organization>(sql, new { id });

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", id);

        // await using var reader = await context.ReaderAsync();

        // return CacheEntity(MapFromReader(reader));
    }

    /// <summary>
    ///     Retrieve all <see cref="Organization"/>.
    /// </summary>
    /// <returns>List of <see cref="Organization"/>.</returns>
    public override async IAsyncEnumerable<Organization> ListAllAsync(Navigation navigation)
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
            yield return CacheEntity(item);
        }

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // await foreach (var reader in context.EnumerableReaderAsync())
        // {
        //     yield return CacheEntity(MapFromReader(reader));
        // }
    }

    /// <summary>
    ///     Update <see cref="Organization"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    public override async Task UpdateAsync(Organization entity)
    {
        ResetCacheEntity(entity);

        var sql = @"
            UPDATE  application.organization
            SET     email = trim(@email)
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { entity.Id, entity.Email });
    }
}
