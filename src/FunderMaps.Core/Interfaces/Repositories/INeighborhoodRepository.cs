using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Neighborhood repository.
/// </summary>
public interface INeighborhoodRepository : IAsyncRepository<Neighborhood, string>
{
    /// <summary>
    ///     Get neighborhood by external identifier.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single neighborhood.</returns>
    Task<Neighborhood> GetByExternalIdAsync(string id);
}
