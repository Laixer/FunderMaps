using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the recovery repository.
/// </summary>
public interface IRecoveryRepository : IAsyncRepository<Recovery, int>
{
    Task DeleteAsync(int id, Guid tenantId);

    Task<Recovery> GetByIdAsync(int id, Guid tenantId);

    IAsyncEnumerable<Recovery> ListAllAsync(Navigation navigation, Guid tenantId);

    /// <summary>
    ///     Set <see cref="Recovery"/> audit status.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="entity">Entity object.</param>
    Task SetAuditStatusAsync(int id, Recovery entity, Guid tenantId);
}
