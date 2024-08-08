using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Repository operations interface.
/// </summary>
/// <typeparam name="TEntity">Derivative of base entity.</typeparam>
/// <typeparam name="TEntityPrimaryKey">Primary key of entity.</typeparam>
public interface IAsyncRepository<TEntity, TEntityPrimaryKey>
    where TEntity : IEntityIdentifier<TEntityPrimaryKey>
{
    /// <summary>
    ///     Retrieve <typeparamref name="TEntity"/> by id.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns><typeparamref name="TEntity"/>.</returns>
    Task<TEntity> GetByIdAsync(TEntityPrimaryKey id);

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
}
