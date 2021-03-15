using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data
{
    /// <summary>
    ///     Generic repository base.
    /// </summary>
    /// <typeparam name="TEntity">Derivative of base entity.</typeparam>
    /// <typeparam name="TEntityPrimaryKey">Primary key of entity.</typeparam>
    internal abstract class RepositoryBase<TEntity, TEntityPrimaryKey> : DbServiceBase, IAsyncRepository<TEntity, TEntityPrimaryKey>
        where TEntity : IdentifiableEntity<TEntity, TEntityPrimaryKey>
        where TEntityPrimaryKey : IEquatable<TEntityPrimaryKey>, IComparable<TEntityPrimaryKey>
    {
        #region Cache

        /// <summary>
        ///     Keypair used as cache bucket item.
        /// </summary>
        protected record CacheKeyPair
        {
            /// <summary>
            ///     Entity hash key.
            /// </summary>
            public int EntityKey { get; init; }

            /// <summary>
            ///     Object hash key.
            /// </summary>
            public int Key { get; init; }

            /// <summary>
            ///     Keypair identifier.
            /// </summary>
            /// <remarks>
            ///     This is very likely to overflow, however the identity shoud
            ///     still be unique per keypair.
            /// </remarks>
            public int KeyPairIdentity => EntityKey + Key;
        }

        /// <summary>
        ///     Build entity hash key.
        /// </summary>
        protected static CacheKeyPair EntityHashKey(object key)
            => new()
            {
                EntityKey = typeof(TEntity).GetHashCode(),
                Key = key.GetHashCode(),
            };

        /// <summary>
        ///     Set cache item.
        /// </summary>
        /// <remarks>
        ///     Derived repositories can override this call to change cache behavior.
        /// </remarks>
        protected virtual void SetCacheItem(CacheKeyPair key, TEntity value, MemoryCacheEntryOptions options)
            => Cache.Set(key.KeyPairIdentity, value, options);

        /// <summary>
        ///     Unset cache item.
        /// </summary>
        /// <remarks>
        ///     Derived repositories can override this call to change cache behavior.
        /// </remarks>
        protected virtual void UnsetCacheItem(CacheKeyPair key)
            => Cache.Remove(key.KeyPairIdentity);

        /// <summary>
        ///     Get cache item.
        /// </summary>
        /// <remarks>
        ///     Derived repositories can override this call to change cache behavior.
        /// </remarks>
        protected virtual bool GetCacheItem(CacheKeyPair key, out TEntity value)
            => Cache.TryGetValue(key.KeyPairIdentity, out value);

        /// <summary>
        ///     Try get entity from cache.
        /// </summary>
        protected bool TryGetEntity(TEntityPrimaryKey key, out TEntity value)
            => GetCacheItem(EntityHashKey(key), out value);

        /// <summary>
        ///     Cache entity.
        /// </summary>
        protected TEntity CacheEntity(TEntityPrimaryKey key, TEntity value)
        {
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            SetCacheItem(EntityHashKey(key), value, options);
            return value;
        }

        /// <summary>
        ///     Cache entity.
        /// </summary>
        protected TEntity CacheEntity(TEntity value)
            => CacheEntity(value.Identifier, value);

        /// <summary>
        ///     Remove entity from cache.
        /// </summary>
        protected void ResetCacheEntity(TEntityPrimaryKey key)
            => UnsetCacheItem(EntityHashKey(key));

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
        /// <param name="alias">Datasource alias.</param>
        /// <returns>The altered SQL query.</returns>
        protected static string ConstructNavigation(string cmdText, Navigation navigation, string alias = null)
        {
            if (navigation is null)
            {
                return cmdText;
            }

            // TODO: This is free-format input with a high potential for bugs/malicious SQL.
            // FUTURE: Can we improve stability and readability here?
            // if (!string.IsNullOrEmpty(navigation.SortColumn))
            // {
            //     var column = alias is not null ? $"{alias}.{navigation.SortColumn}" : navigation.SortColumn;
            //     cmdText += $"\r\n ORDER BY {column} {(navigation.SortOrder == SortOrder.Ascending ? "ASC" : "DESC")}";
            // }

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
        public abstract Task<TEntityPrimaryKey> AddAsync(TEntity entity);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.UpdateAsync"/>
        /// </summary>
        public abstract Task UpdateAsync(TEntity entity);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.DeleteAsync"/>
        /// </summary>
        public abstract Task DeleteAsync(TEntityPrimaryKey id);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.CountAsync"/>
        /// </summary>
        public abstract Task<long> CountAsync();
    }
}
