using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Building repository.
/// </summary>
public interface IBuildingRepository : IAsyncRepository<Building, string>
{
    /// <summary>
    ///     Retrieve building by identifier.
    /// </summary>
    Task<Building> GetByIdAsync(string id);

    /// <summary>
    ///     Get building by external id.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single building.</returns>
    Task<Building> GetByExternalIdAsync(string id);

    /// <summary>
    ///     Get building by external address id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single building.</returns>
    Task<Building> GetByExternalAddressIdAsync(string id);

    Task<Building> GetByIncidentIdAsync(string id);
}
