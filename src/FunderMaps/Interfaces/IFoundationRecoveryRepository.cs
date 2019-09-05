using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Operations for the foundation recovery repository.
    /// </summary>
    public interface IFoundationRecoveryRepository : IAsyncRepository<FoundationRecovery, int>
    {
        /// <summary>
        /// Retrieve entity by id and organization.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns><see cref="FoundationRecovery"/> on success, null on error.</returns>
        Task<FoundationRecovery> GetByIdAsync(int id, Guid orgId);

        /// <summary>
        /// Retrieve entity by id and organization or public record.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns><see cref="FoundationRecovery"/> on success, null on error.</returns>
        Task<FoundationRecovery> GetPublicAndByIdAsync(int id, Guid orgId);

        /// <summary>
        /// Return a list of items filterd by the organization id and the navigation values
        /// </summary>
        /// <param name="orgId">The id of the organization.</param>
        /// <param name="navigation">The Navigation values.</param>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<FoundationRecovery>> ListAllAsync(Guid orgId, Navigation navigation);

        /// <summary>
        /// Retrieve number of entities and filter on organization id.
        /// </summary>
        /// <returns>Number of entities.</returns>
        Task<uint> CountAsync(Guid orgId);
    }
}
