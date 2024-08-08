using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Municipality repository.
/// </summary>
public interface IMunicipalityRepository
{
    /// <summary>
    ///     Retrieve municipality by identifier.
    /// </summary>
    Task<Municipality> GetByIdAsync(string id);

    /// <summary>
    ///     Get municipality by external identifier.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single municipality.</returns>
    Task<Municipality> GetByExternalIdAsync(string id);

    /// <summary>
    ///     Get municipality by external address id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single municipality.</returns>
    Task<Municipality> GetByExternalAddressIdAsync(string id);

    /// <summary>
    ///     Get municipality by external building id.
    /// </summary>
    /// <param name="id">External address identifier.</param>
    /// <returns>A single municipality.</returns>
    Task<Municipality> GetByExternalBuildingIdAsync(string id);

    /// <summary>
    ///    Get municipality by external neighborhood id.
    /// </summary>
    /// <param name="id">External neighborhood identifier.</param>
    /// <returns>A single municipality.</returns>
    Task<Municipality> GetByExternalNeighborhoodIdAsync(string id);

    /// <summary>
    ///   Get municipality by external district id.
    /// </summary>
    /// <param name="id">External district identifier.</param>
    /// <returns>A single municipality.</returns>
    Task<Municipality> GetByExternalDistrictIdAsync(string id);
}
