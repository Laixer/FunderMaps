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
        if (value.Id is null)
        {
            return value;
        }

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(90));

        Cache.Set(value.Id, value, options);
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
        => ResetCacheEntity(value.Id);

    #endregion Cache

    // TODO: Remove
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

    /// <summary>
    ///     <see cref="IAsyncRepository{TEntry, TEntityPrimaryKey}.GetByIdAsync"/>
    /// </summary>
    public virtual Task<TEntity> GetByIdAsync(TEntityPrimaryKey id)
        => throw new InvalidOperationException();

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
