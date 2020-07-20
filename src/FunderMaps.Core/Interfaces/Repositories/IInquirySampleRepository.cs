using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    /// Operations for the inquiry sample repository.
    /// </summary>
    public interface IInquirySampleRepository : IAsyncRepository<InquirySample, int>
    {
        /// <summary>
        /// Retrieve entity by id and organization.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns><see cref="InquirySample"/> on success, null on error.</returns>
        Task<InquirySample> GetByIdAsync(int id, Guid orgId);

        /// <summary>
        /// Retrieve entity by id and organization or public record.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns><see cref="InquirySample"/> on success, null on error.</returns>
        Task<InquirySample> GetPublicAndByIdAsync(int id, Guid orgId);

        /// <summary>
        /// Retrieve all entities and filter on organization id.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<InquirySample>> ListAllAsync(Guid orgId, INavigation navigation);

        /// <summary>
        /// Retrieve all entities and filter on report.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<InquirySample>> ListAllReportAsync(int report, INavigation navigation);

        /// <summary>
        /// Retrieve all entities and filter on report and organization id.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IReadOnlyList<InquirySample>> ListAllReportAsync(int report, Guid orgId, INavigation navigation);

        /// <summary>
        /// Retrieve number of entities and filter on organization id.
        /// </summary>
        /// <returns>Number of entities.</returns>
        Task<uint> CountAsync(Guid orgId);
    }
}
