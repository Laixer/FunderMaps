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
    /// <typeparam name="TEntryPrimaryKey">Primary key of entity.</typeparam>
    public interface IAsyncRepository<TEntity, TEntryPrimaryKey>
        where TEntity : BaseEntity
        where TEntryPrimaryKey : IEquatable<TEntryPrimaryKey>
    {
        /// <summary>
        ///     Retrieve <typeparamref name="TEntity"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><typeparamref name="TEntity"/>.</returns>
        ValueTask<TEntity> GetByIdAsync(TEntryPrimaryKey id);

        /// <summary>
        ///     Retrieve all <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>List of <typeparamref name="TEntity"/>.</returns>
        IAsyncEnumerable<TEntity> ListAllAsync(INavigation navigation);

        /// <summary>
        ///     Create new <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <typeparamref name="TEntity"/>.</returns>
        ValueTask<TEntryPrimaryKey> AddAsync(TEntity entity);

        /// <summary>
        ///     Update <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        ValueTask UpdateAsync(TEntity entity);

        // TODO: Delete based on id.
        /// <summary>
        ///     Delete <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        ValueTask DeleteAsync(TEntryPrimaryKey id);

        /// <summary>
        ///     Count number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        ValueTask<ulong> CountAsync();
    }
}
