using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Address repository.
/// </summary>
public interface IAddressRepository
{
    /// <summary>
    ///     Get address by external id.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single address.</returns>
    Task<Address> GetByExternalIdAsync(string id);

    /// <summary>
    ///     Get address by external id.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single address.</returns>
    Task<Address> GetByExternalBuildingIdAsync(string id);
}
