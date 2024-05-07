using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Residence repository.
/// </summary>
public interface IResidenceRepository : IAsyncRepository<Residence, string>
{
    /// <summary>
    ///     Get residence by external address id.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single residence.</returns>
    Task<Residence> GetByExternalAddressIdAsync(string id);

    /// <summary>
    ///     Get residence by external building id.
    /// </summary>
    /// <param name="id">External identifier.</param>
    /// <returns>A single residence.</returns>
    Task<Residence> GetByExternalBuildingIdAsync(string id);
}
