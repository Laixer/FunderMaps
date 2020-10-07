using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
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
    internal abstract class RepositoryBase<TEntity, TEntityPrimaryKey> : DbContextBase, IAsyncRepository<TEntity, TEntityPrimaryKey>
        where TEntity : IdentifiableEntity<TEntity, TEntityPrimaryKey>
        where TEntityPrimaryKey : IEquatable<TEntityPrimaryKey>, IComparable<TEntityPrimaryKey>
    {
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
