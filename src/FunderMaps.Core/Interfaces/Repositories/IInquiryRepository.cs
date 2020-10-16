using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Operations for the inquiry repository.
    /// </summary>
    public interface IInquiryRepository : IAsyncRepository<InquiryFull, int>
    {
        /// <summary>
        ///     Retrieve entity by id and document_id and organization or public record.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>Entity.</returns>
        Task<InquiryFull> GetPublicAndByIdAsync(int id, Guid orgId);

        /// <summary>
        ///     Set <see cref="InquiryFull"/> audit status.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <param name="entity">Entity object.</param>
        Task SetAuditStatusAsync(int id, InquiryFull entity);
    }
}
