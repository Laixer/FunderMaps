using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the recovery sample repository.
/// </summary>
public interface IRecoverySampleRepository : IAsyncRepository<RecoverySample, int>
{
    IAsyncEnumerable<RecoverySample> ListAllByBuildingIdAsync(string id);

    /// <summary>
    ///     Retrieve number of <see cref="RecoverySample"/> for a given <see cref="Recovery"/>.
    /// </summary>
    /// <returns>Number of <see cref="RecoverySample"/>.</returns>
    Task<long> CountAsync(int recovery, Guid tenantId);

    Task<RecoverySample> GetByIdAsync(int id, Guid tenantId);

    Task UpdateAsync(RecoverySample entity, Guid tenantId);

    Task DeleteAsync(int id, Guid tenantId);

    /// <summary>
    ///     Retrieve all <see cref="RecoverySample"/> for a <see cref="Recovery"/>.
    /// </summary>
    /// <returns>List of <see cref="RecoverySample"/>.</returns>
    IAsyncEnumerable<RecoverySample> ListAllAsync(int recovery, Navigation navigation, Guid tenantId);

    IAsyncEnumerable<RecoverySample> ListAllAsync(Navigation navigation, Guid tenantId);
}
