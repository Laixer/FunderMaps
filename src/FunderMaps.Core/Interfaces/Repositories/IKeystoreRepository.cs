using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the keystore repository.
/// </summary>
public interface IKeystoreRepository : IAsyncRepository<KeyStore, string>
{
    /// <summary>
    ///     Create new key store.
    /// </summary>
    Task<string> AddAsync(KeyStore entity);

    /// <summary>
    ///     Retrieve all key stores.
    /// </summary>
    IAsyncEnumerable<KeyStore> ListAllAsync();
}
