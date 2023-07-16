using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Address repository.
/// </summary>
public interface IAddressRepository : IAsyncRepository<Address, string>
{
    /// <summary>
    ///     Get address by external id.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single address.</returns>
    Task<Address> GetByExternalIdAsync(string id);
}
