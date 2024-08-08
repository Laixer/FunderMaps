using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Neighborhood repository.
/// </summary>
public interface INeighborhoodRepository : IAsyncRepository<Neighborhood, string>
{
    /// <summary>
    ///     Get neighborhood by identifier.
    /// </summary>
    Task<Neighborhood> GetByIdAsync(string id);

    /// <summary>
    ///     Get neighborhood by external identifier.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single neighborhood.</returns>
    Task<Neighborhood> GetByExternalIdAsync(string id);

    /// <summary>
    ///     Get neighborhood by external address id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single neighborhood.</returns>
    Task<Neighborhood> GetByExternalAddressIdAsync(string id);

    /// <summary>
    ///     Get neighborhood by external building id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single neighborhood.</returns>
    Task<Neighborhood> GetByExternalBuildingIdAsync(string id);
}
