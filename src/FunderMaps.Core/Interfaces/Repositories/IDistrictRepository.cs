using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     District repository.
/// </summary>
public interface IDistrictRepository : IAsyncRepository<District, string>
{
    /// <summary>
    ///     Get district by external identifier.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single district.</returns>
    Task<District> GetByExternalIdAsync(string id);

    /// <summary>
    ///     Get district by external address id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single district.</returns>
    Task<District> GetByExternalAddressIdAsync(string id);

    /// <summary>
    ///     Get district by external building id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single district.</returns>
    Task<District> GetByExternalBuildingIdAsync(string id);

    /// <summary>
    ///    Get district by external neighborhood id.
    /// </summary>
    /// <param name="id">External neighborhood identifier.</param>
    /// <returns>A single district.</returns>
    Task<District> GetByExternalNeighborhoodIdAsync(string id);
}
