using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using System.Data.Common;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Bundle repository.
/// </summary>
internal class BundleRepository : RepositoryBase<Bundle, string>, IBundleRepository
{
    public override Task<string> AddAsync(Bundle entity)
        => throw new InvalidOperationException();

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public override async Task<long> CountAsync()
    {
        var cmd = CountCommand("maplayer");

        await using var context = await DbContextFactory.CreateAsync(cmd);

        return await context.ScalarAsync<long>();
    }

    /// <summary>
    ///     Delete <see cref="Bundle"/>.
    /// </summary>
    /// <param name="id">Entity id.</param>
    public override async Task DeleteAsync(string id)
    {
        ResetCacheEntity(id);

        var cmd = DeleteCommand("application", "tileset");

        await using var context = await DbContextFactory.CreateAsync(cmd);

        context.AddParameterWithValue("tileset", id);

        await context.NonQueryAsync();
    }

    private static Bundle MapFromReader(DbDataReader reader, int offset = 0)
        => new()
        {
            Tileset = reader.GetString(offset++),
            Enabled = reader.GetBoolean(offset++),
            BuiltDate = reader.GetSafeDateTime(offset++),
            Precondition = reader.GetSafeString(offset++),
            Name = reader.GetString(offset++),
            MinZoomLevel = reader.GetInt(offset++),
            MaxZoomLevel = reader.GetInt(offset++),
        };

    /// <summary>
    ///     Retrieve <see cref="Bundle"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Bundle"/>.</returns>
    public override async Task<Bundle> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out Bundle entity))
        {
            return entity;
        }

        var entityName = EntityTable("maplayer");

        var cmd = $@"
        SELECT
            tileset,
            enabled,
            built_date,
            precondition,
            name,
            zoom_min_level,
            zoom_max_level
        FROM {entityName}
        WHERE tileset = @tileset
        LIMIT 1";

        await using var context = await DbContextFactory.CreateAsync(cmd);

        context.AddParameterWithValue("tileset", id);

        await using var reader = await context.ReaderAsync();

        return CacheEntity(MapFromReader(reader));
    }

    /// <summary>
    ///     Retrieve all <see cref="Bundle"/>.
    /// </summary>
    /// <returns>List of <see cref="Bundle"/>.</returns>
    public override async IAsyncEnumerable<Bundle> ListAllAsync(Navigation navigation)
    {
        var cmd = AllCommand("maplayer", new[] { "tileset", "enabled", "built_date", "precondition", "name", "zoom_min_level", "zoom_max_level" }, navigation);

        await using var context = await DbContextFactory.CreateAsync(cmd);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }

    /// <summary>
    ///     Retrieve all enabled <see cref="Bundle"/>.
    /// </summary>
    /// <returns>List of <see cref="Bundle"/>.</returns>
    public async IAsyncEnumerable<Bundle> ListAllEnabledAsync()
    {
        var entityName = EntityTable("maplayer");

        var cmd = $@"
            SELECT
                tileset,
                enabled,
                built_date,
                precondition,
                name,
                zoom_min_level,
                zoom_max_level
            FROM {entityName}
            WHERE enabled
            ORDER BY built_date ASC NULLS FIRST";

        await using var context = await DbContextFactory.CreateAsync(cmd);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return CacheEntity(MapFromReader(reader));
        }
    }

    /// <summary>
    ///     Run precondition.
    /// </summary>
    public async Task<bool> RunPreconditionAsync(string id, string precondition)
    {
        var cmd = $@"
            SELECT  maplayer.{precondition}(built_date) > precondition_threshold AS precondition
            FROM    maplayer.bundle
            WHERE   tileset = @tileset";

        await using var context = await DbContextFactory.CreateAsync(cmd);

        context.AddParameterWithValue("tileset", id);

        return await context.ScalarAsync<bool>();
    }

    /// <summary>
    ///     Log the built time of a bundle.
    /// </summary>
    public async Task LogBuiltTimeAsync(string id)
    {
        var entityName = EntityTable("maplayer");

        var cmd = $@"
            UPDATE {entityName}
            SET built_date = NOW()
            WHERE tileset = @tileset";

        await using var context = await DbContextFactory.CreateAsync(cmd);

        context.AddParameterWithValue("tileset", id);

        await context.NonQueryAsync();
    }

    /// <summary>
    ///     Update <see cref="Bundle"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    public override Task UpdateAsync(Bundle entity)
        => throw new InvalidOperationException();
}
