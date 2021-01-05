using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Repository operations interface.
    /// </summary>
    /// <typeparam name="TEntity">Derivative of base entity.</typeparam>
    /// <typeparam name="TEntityPrimaryKey">Primary key of entity.</typeparam>
    public interface IAsyncRepository<TEntity, TEntityPrimaryKey>
        where TEntity : IdentifiableEntity<TEntity, TEntityPrimaryKey>
        where TEntityPrimaryKey : IEquatable<TEntityPrimaryKey>, IComparable<TEntityPrimaryKey>
    {
        /// <summary>
        ///     Create and return <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> AddGetAsync(TEntity entity);

        /// <summary>
        ///     Retrieve <typeparamref name="TEntity"/> by id.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns><typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetByIdAsync(TEntityPrimaryKey id);

        /// <summary>
        ///     Retrieve all <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>List of <typeparamref name="TEntity"/>.</returns>
        IAsyncEnumerable<TEntity> ListAllAsync(INavigation navigation);

        /// <summary>
        ///     Create new <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Entity identifier.</returns>
        Task<TEntityPrimaryKey> AddAsync(TEntity entity);

        /// <summary>
        ///     Update <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        ///     Delete <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        Task DeleteAsync(TEntityPrimaryKey id);

        /// <summary>
        ///     Count number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        Task<long> CountAsync();
    }
}
