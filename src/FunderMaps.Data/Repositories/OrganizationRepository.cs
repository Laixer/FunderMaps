using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Organization repository.
/// </summary>
internal class OrganizationRepository : RepositoryBase<Organization, Guid>, IOrganizationRepository
{
    public async Task<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash)
    {
        var sql = @"
            SELECT application.create_organization(
                @id,
                @email,
                @passwordHash)";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);
        context.AddParameterWithValue("email", email);
        context.AddParameterWithValue("passwordHash", passwordHash);

        await using var reader = await context.ReaderAsync();

        return reader.GetGuid(0);
    }

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    application.organization";

        await using var context = await DbContextFactory.CreateAsync(sql);

        return await context.ScalarAsync<long>();
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", id);

        await context.NonQueryAsync();
    }

    private static Organization MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Id = reader.GetGuid(offset++),
            Name = reader.GetString(offset++),
            Email = reader.GetString(offset++),
            Area = new()
            {
                XMin = reader.GetSafeDouble(offset++),
                YMin = reader.GetSafeDouble(offset++),
                XMax = reader.GetSafeDouble(offset++),
                YMax = reader.GetSafeDouble(offset++),
            },
            Center = new()
            {
                CenterX = reader.GetSafeDouble(offset++),
                CenterY = reader.GetSafeDouble(offset++),
            }
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
                    email,
                    st_xmin(fence) AS x_min,
                    st_ymin(fence) AS y_min,
                    st_xmax(fence) AS x_max,
                    st_ymax(fence) AS y_max,
                    st_x(st_centroid(fence)) AS center_x,
                    st_y(st_centroid(fence)) AS center_y
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
                    st_xmin(fence) AS x_min,
                    st_ymin(fence) AS y_min,
                    st_xmax(fence) AS x_max,
                    st_ymax(fence) AS y_max,
                    st_x(st_centroid(fence)) AS center_x,
                    st_y(st_centroid(fence)) AS center_y
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("id", entity.Id);
        context.AddParameterWithValue("email", entity.Email);

        await context.NonQueryAsync();
    }
}
