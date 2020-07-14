using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    /// Operations for the inquiry repository.
    /// </summary>
    public interface IInquiryRepository : IAsyncRepository<Inquiry, int>
    {
        /// <summary>
        /// Retrieve entity by id and document_id and organization.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>Entity.</returns>
        Task<Inquiry> GetByIdAsync(int id, Guid orgId);

        /// <summary>
        /// Retrieve entity by id and document_id and organization or public record.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>Entity.</returns>
        Task<Inquiry> GetPublicAndByIdAsync(int id, Guid orgId);

        /// <summary>
        /// Retrieve all entities and filter on organization id.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<Inquiry>> ListAllAsync(Guid orgId, INavigation navigation);

        /// <summary>
        /// Retrieve number of entities and filter on organization id.
        /// </summary>
        /// <returns>Number of entities.</returns>
        Task<uint> CountAsync(Guid orgId);
    }
}
