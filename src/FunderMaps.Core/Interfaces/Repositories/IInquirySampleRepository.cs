using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Operations for the inquiry sample repository.
    /// </summary>
    public interface IInquirySampleRepository : IAsyncRepository<InquirySample, int>
    {
        /// <summary>
        ///     Retrieve entity by id and organization or public record.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns><see cref="InquirySample"/> on success, null on error.</returns>
        Task<InquirySample> GetPublicAndByIdAsync(int id, Guid orgId);

        /// <summary>
        ///     Retrieve number of entities and filter on report.
        /// </summary>
        /// <returns>Number of entities.</returns>
        Task<long> CountAsync(int report);

        /// <summary>
        ///     Retrieve all entities and filter on report.
        /// </summary>
        /// <returns>List of entities.</returns>
        IAsyncEnumerable<InquirySample> ListAllAsync(int report, INavigation navigation);
    }
}
