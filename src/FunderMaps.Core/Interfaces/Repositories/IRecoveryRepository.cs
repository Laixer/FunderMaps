using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the recovery repository.
/// </summary>
public interface IRecoveryRepository
{
    /// <summary>
    ///     Add new recovery.
    /// </summary>
    Task<int> AddAsync(Recovery entity);

    /// <summary>
    ///     Count number of recoveries.
    /// </summary>
    Task<long> CountAsync();

    Task DeleteAsync(int id, Guid tenantId);

    Task UpdateAsync(Recovery entity);

    Task<Recovery> GetByIdAsync(int id, Guid tenantId);

    IAsyncEnumerable<Recovery> ListAllAsync(Navigation navigation, Guid tenantId);

    /// <summary>
    ///     Set <see cref="Recovery"/> audit status.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="entity">Entity object.</param>
    Task SetAuditStatusAsync(int id, Recovery entity, Guid tenantId);

    IAsyncEnumerable<Recovery> ListAllByBuildingIdAsync(Navigation navigation, Guid tenantId, string id);
}
