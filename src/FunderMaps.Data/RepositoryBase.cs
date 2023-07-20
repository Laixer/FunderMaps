using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace FunderMaps.Data;

/// <summary>
///     Generic repository base.
/// </summary>
/// <typeparam name="TEntity">Derivative of base entity.</typeparam>
/// <typeparam name="TEntityPrimaryKey">Primary key of entity.</typeparam>
internal abstract class RepositoryBase<TEntity, TEntityPrimaryKey> : DbServiceBase, IAsyncRepository<TEntity, TEntityPrimaryKey>
    where TEntity : IEntityIdentifier<TEntityPrimaryKey>
{
    #region Cache

    /// <summary>
    ///     Try get entity from cache.
    /// </summary>
    protected bool TryGetEntity(TEntityPrimaryKey key, out TEntity? value)
    {
        if (key is null)
        {
            value = default;
            return false;
        }

        return Cache.TryGetValue(key, out value);
    }

    /// <summary>
    ///     Cache entity.
    /// </summary>
    protected TEntity CacheEntity(TEntity value)
    {
        if (value.Identifier is null)
        {
            return value;
        }

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(90));

        Cache.Set(value.Identifier, value, options);
        return value;
    }

    /// <summary>
    ///     Remove entity from cache.
    /// </summary>
    protected void ResetCacheEntity(TEntityPrimaryKey key)
    {
        if (key is not null)
        {
            Cache.Remove(key);
        }
    }

    /// <summary>
    ///     Remove entity from cache.
    /// </summary>
    protected void ResetCacheEntity(TEntity value)
        => ResetCacheEntity(value.Identifier);

    #endregion Cache

    // FUTURE: Maybe too npgsql specific.
    // FUTURE: Extension ?
    /// <summary>
    ///     Convert navigation to query.
    /// </summary>
    /// <param name="cmdText">SQL query.</param>
    /// <param name="navigation">Navigation instance of type <see cref="Navigation"/>.</param>
    /// <returns>The altered SQL query.</returns>
    protected static string ConstructNavigation(string cmdText, Navigation navigation)
    {
        if (navigation is null)
        {
            return cmdText;
        }

        if (navigation.Offset > 0)
        {
            cmdText += $"\r\n OFFSET {navigation.Offset}";
        }

        if (navigation.Limit > 0)
        {
            cmdText += $"\r\n LIMIT {navigation.Limit}";
        }

        return cmdText;
    }

    protected static string EntityTable(string? schema = null)
    {
        var table = typeof(TEntity).Name.ToLowerInvariant();

        if (schema is not null)
        {
            table = $"{schema}.{table}";
        }

        return table;
    }

    protected static string CountCommand(string? schema = null)
    {
        var entityName = EntityTable(schema);

        var sql = $@"SELECT COUNT(*) FROM {entityName} LIMIT 1";

        return sql;
    }

    protected static string DeleteCommand(string schema, string column)
    {
        var entityName = EntityTable(schema);

        var sql = $@"DELETE FROM {entityName} WHERE {column} = @id";

        return sql;
    }

    protected static string SingleCommand(string schema, string[] columns)
    {
        var entityName = EntityTable(schema);

        var sql = $@"SELECT {string.Join(", ", columns)} FROM {entityName} WHERE {columns[0]} = @id LIMIT 1";

        return sql;
    }

    protected static string AllCommand(string schema, string[] columns, Navigation navigation)
    {
        var entityName = EntityTable(schema);

        var sql = $@"SELECT {string.Join(", ", columns)} FROM {entityName}";

        sql = ConstructNavigation(sql, navigation);

        return sql;
    }

    /// <summary>
    ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.AddGetAsync"/>
    /// </summary>
    public virtual async Task<TEntity> AddGetAsync(TEntity entity)
    {
        TEntityPrimaryKey primaryKey = await AddAsync(entity);
        return await GetByIdAsync(primaryKey);
    }

    /// <summary>
    ///     <see cref="IAsyncRepository{TEntry, TEntityPrimaryKey}.GetByIdAsync"/>
    /// </summary>
    public abstract Task<TEntity> GetByIdAsync(TEntityPrimaryKey id);

    /// <summary>
    ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.ListAllAsync"/>
    /// </summary>
    public abstract IAsyncEnumerable<TEntity> ListAllAsync(Navigation navigation);

    /// <summary>
    ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.AddAsync"/>
    /// </summary>
    public virtual Task<TEntityPrimaryKey> AddAsync(TEntity entity)
        => throw new InvalidOperationException();

    /// <summary>
    ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.UpdateAsync"/>
    /// </summary>
    public virtual Task UpdateAsync(TEntity entity)
        => throw new InvalidOperationException();

    /// <summary>
    ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.DeleteAsync"/>
    /// </summary>
    public virtual Task DeleteAsync(TEntityPrimaryKey id)
        => throw new InvalidOperationException();

    /// <summary>
    ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.CountAsync"/>
    /// </summary>
    public abstract Task<long> CountAsync();
}
