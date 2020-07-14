using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Repository operations interface.
    /// </summary>
    /// <typeparam name="TEntry">Derivative of base entity.</typeparam>
    /// <typeparam name="TEntryPrimaryKey">Primary key of entity.</typeparam>
    public interface IAsyncRepository<TEntry, TEntryPrimaryKey>
    {
        /// <summary>
        ///     Retrieve <typeparamref name="TEntry"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><typeparamref name="TEntry"/>.</returns>
        ValueTask<TEntry> GetByIdAsync(TEntryPrimaryKey id);

        /// <summary>
        ///     Retrieve all <typeparamref name="TEntry"/>.
        /// </summary>
        /// <returns>List of <typeparamref name="TEntry"/>.</returns>
        IAsyncEnumerable<TEntry> ListAllAsync(INavigation navigation);

        /// <summary>
        ///     Create new <typeparamref name="TEntry"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <typeparamref name="TEntry"/>.</returns>
        ValueTask<TEntryPrimaryKey> AddAsync(TEntry entity);

        /// <summary>
        ///     Update <typeparamref name="TEntry"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        ValueTask UpdateAsync(TEntry entity);

        // TODO: Delete based on id.
        /// <summary>
        ///     Delete <typeparamref name="TEntry"/>.
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
