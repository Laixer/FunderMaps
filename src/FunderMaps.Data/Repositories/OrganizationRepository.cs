using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using System.Data.Common;

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

    private static Organization MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Id = reader.GetGuid(offset++),
            Name = reader.GetString(offset++),
            Email = reader.GetString(offset++),
            // Area = new()
            // {
            //     XMin = reader.GetSafeDouble(offset++),
            //     YMin = reader.GetSafeDouble(offset++),
            //     XMax = reader.GetSafeDouble(offset++),
            //     YMax = reader.GetSafeDouble(offset++),
            // },
            // Center = new()
            // {
            //     CenterX = reader.GetSafeDouble(offset++),
            //     CenterY = reader.GetSafeDouble(offset++),
            // }
        };

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
                    -- 0 AS x_min,
                    -- 0 AS y_min,
                    -- 0 AS x_max,
                    -- 0 AS y_max,
                    -- 0 AS center_x,
                    -- 0 AS center_y
            FROM    application.organization
            WHERE   id = @id
            LIMIT   1";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await using var reader = await context.ReaderAsync();

        return CacheEntity(MapFromReader(reader));
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
                    email,
                    -- 0 AS x_min,
                    -- 0 AS y_min,
                    -- 0 AS x_max,
                    -- 0 AS y_max,
                    -- 0 AS center_x,
                    -- 0 AS center_y
            FROM    application.organization";

        sql = ConstructNavigation(sql, navigation);

        await using var context = await DbContextFactory.CreateAsync(sql);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
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
