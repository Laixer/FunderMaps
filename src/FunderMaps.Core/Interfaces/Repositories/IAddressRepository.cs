using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;

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
    /// <param name="source">External source.</param>
    /// <returns>A single address.</returns>
    Task<Address> GetByExternalIdAsync(string id, ExternalDataSource source);
}
