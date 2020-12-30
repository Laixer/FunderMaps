using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data
{
    // FUTURE: Detach memory from AppContext
    /// <summary>
    ///     Generic repository base.
    /// </summary>
    /// <typeparam name="TEntity">Derivative of base entity.</typeparam>
    /// <typeparam name="TEntityPrimaryKey">Primary key of entity.</typeparam>
    internal abstract class RepositoryBase<TEntity, TEntityPrimaryKey> : DbContextBase, IAsyncRepository<TEntity, TEntityPrimaryKey>
        where TEntity : IdentifiableEntity<TEntity, TEntityPrimaryKey>
        where TEntityPrimaryKey : IEquatable<TEntityPrimaryKey>, IComparable<TEntityPrimaryKey>
    {
        // FUTURE: Single transaction
        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.AddGetAsync"/>
        /// </summary>
        public virtual async Task<TEntity> AddGetAsync(TEntity entity)
        {
            TEntityPrimaryKey primaryKey = await AddAsync(entity);
            return await GetByIdAsync(primaryKey);
        }

        /// <summary>
        ///     Keypair used as cache identifier.
        /// </summary>
        protected class KeyPair
        {
            /// <summary>
            ///     Entity hash key.
            /// </summary>
            public int EntityKey { get; set; }

            /// <summary>
            ///     Object hash key.
            /// </summary>
            public int Key { get; set; }

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
        protected static KeyPair EntityHashKey(object key)
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
        protected virtual void SetCacheItem(KeyPair key, TEntity value, MemoryCacheEntryOptions options)
            => AppContext.Cache.Set(key.KeyPairIdentity, value, options);

        /// <summary>
        ///     Unset cache item.
        /// </summary>
        /// <remarks>
        ///     Derived repositories can override this call to change cache behavior.
        /// </remarks>
        protected virtual void UnsetCacheItem(KeyPair key)
            => AppContext.Cache.Remove(key.KeyPairIdentity);

        /// <summary>
        ///     Get cache item.
        /// </summary>
        /// <remarks>
        ///     Derived repositories can override this call to change cache behavior.
        /// </remarks>
        protected virtual bool GetCacheItem(KeyPair key, out TEntity value)
            => AppContext.Cache.TryGetValue(key.KeyPairIdentity, out value);

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

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntry, TEntityPrimaryKey}.GetByIdAsync"/>
        /// </summary>
        public abstract ValueTask<TEntity> GetByIdAsync(TEntityPrimaryKey id);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.ListAllAsync"/>
        /// </summary>
        public abstract IAsyncEnumerable<TEntity> ListAllAsync(INavigation navigation);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.AddAsync"/>
        /// </summary>
        public abstract ValueTask<TEntityPrimaryKey> AddAsync(TEntity entity);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.UpdateAsync"/>
        /// </summary>
        public abstract ValueTask UpdateAsync(TEntity entity);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.DeleteAsync"/>
        /// </summary>
        public abstract ValueTask DeleteAsync(TEntityPrimaryKey id);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.CountAsync"/>
        /// </summary>
        public abstract ValueTask<long> CountAsync();
    }
}
