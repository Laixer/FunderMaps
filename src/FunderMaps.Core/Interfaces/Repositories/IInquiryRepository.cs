using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the inquiry repository.
/// </summary>
public interface IInquiryRepository : IAsyncRepository<Inquiry, int>
{
    /// <summary>
    ///     Set <see cref="InquiryFull"/> audit status.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="entity">Entity object.</param>
    Task SetAuditStatusAsync(int id, Inquiry entity);
}
