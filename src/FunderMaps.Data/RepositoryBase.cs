using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data;

/// <summary>
///     Generic repository base.
/// </summary>
/// <typeparam name="TEntity">Derivative of base entity.</typeparam>
/// <typeparam name="TEntityPrimaryKey">Primary key of entity.</typeparam>
internal abstract class RepositoryBase<TEntity, TEntityPrimaryKey> : DbServiceBase, IAsyncRepository<TEntity, TEntityPrimaryKey>
    where TEntity : IEntityIdentifier<TEntityPrimaryKey>
{
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
}
