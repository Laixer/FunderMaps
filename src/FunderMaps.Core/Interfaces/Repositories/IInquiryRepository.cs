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
        ///     Set <see cref="InquiryFull"/> audit status.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <param name="entity">Entity object.</param>
        Task SetAuditStatusAsync(int id, InquiryFull entity);
    }
}
