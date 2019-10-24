using FunderMaps.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    /// Repository operations interface.
    /// </summary>
    /// <typeparam name="TEntry">Derivative of base entity.</typeparam>
    /// <typeparam name="TEntryPrimaryKey">Primary key of entity.</typeparam>
    public interface IAsyncRepository<TEntry, TEntryPrimaryKey>
    {
        /// <summary>
        /// Retrieve <typeparamref name="TEntry"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><typeparamref name="TEntry"/>.</returns>
        Task<TEntry> GetByIdAsync(TEntryPrimaryKey id);

        /// <summary>
        /// Retrieve all <typeparamref name="TEntry"/>.
        /// </summary>
        /// <returns>List of <typeparamref name="TEntry"/>.</returns>
        Task<IReadOnlyList<TEntry>> ListAllAsync(Navigation navigation);

        /// <summary>
        /// Create new <typeparamref name="TEntry"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <typeparamref name="TEntry"/>.</returns>
        Task<TEntryPrimaryKey> AddAsync(TEntry entity);

        /// <summary>
        /// Update <typeparamref name="TEntry"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        Task UpdateAsync(TEntry entity);

        /// <summary>
        /// Delete <typeparamref name="TEntry"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        Task DeleteAsync(TEntry entity);

        /// <summary>
        /// Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        Task<uint> CountAsync();
    }
}
