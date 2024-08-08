using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     State repository.
/// </summary>
public interface IStateRepository
{
    /// <summary>
    ///     Retrieve state by identifier.
    /// </summary>
    Task<State> GetByIdAsync(string id);

    /// <summary>
    ///     Get state by external identifier.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single state.</returns>
    Task<State> GetByExternalIdAsync(string id);

    /// <summary>
    ///     Get state by external address id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single state.</returns>
    Task<State> GetByExternalAddressIdAsync(string id);

    /// <summary>
    ///     Get state by external building id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single state.</returns>
    Task<State> GetByExternalBuildingIdAsync(string id);

    /// <summary>
    ///    Get state by external neighborhood id.
    /// </summary>
    /// <param name="id">External neighborhood identifier.</param>
    /// <returns>A single state.</returns>
    Task<State> GetByExternalNeighborhoodIdAsync(string id);

    /// <summary>
    ///    Get state by external district id.
    /// </summary>
    /// <param name="id">External district identifier.</param>
    /// <returns>A single state.</returns>
    Task<State> GetByExternalDistrictIdAsync(string id);

    /// <summary>
    ///    Get state by external municipality id.
    /// </summary>
    /// <param name="id">External municipality identifier.</param>
    /// <returns>A single state.</returns>
    Task<State> GetByExternalMunicipalityIdAsync(string id);
}
