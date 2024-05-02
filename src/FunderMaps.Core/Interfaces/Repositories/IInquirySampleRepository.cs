using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the inquiry sample repository.
/// </summary>
public interface IInquirySampleRepository : IAsyncRepository<InquirySample, int>
{
    // TODO: Add tenantId
    IAsyncEnumerable<InquirySample> ListAllByBuildingIdAsync(string id);

    // TOOD: Remove
    Task<long> CountAsync(Guid tenantId);

    /// <summary>
    ///     Retrieve number of entities and filter on report.
    /// </summary>
    /// <returns>Number of entities.</returns>
    Task<long> CountAsync(int report, Guid tenantId);

    Task DeleteAsync(int id, Guid tenantId);

    Task<InquirySample> GetByIdAsync(int id, Guid tenantId);

    // TOOD: Remove
    IAsyncEnumerable<InquirySample> ListAllAsync(Navigation navigation, Guid tenantId);

    /// <summary>
    ///     Retrieve all entities and filter on report.
    /// </summary>
    /// <returns>List of entities.</returns>
    IAsyncEnumerable<InquirySample> ListAllAsync(int report, Navigation navigation, Guid tenantId);

    Task UpdateAsync(InquirySample entity, Guid tenantId);
}
