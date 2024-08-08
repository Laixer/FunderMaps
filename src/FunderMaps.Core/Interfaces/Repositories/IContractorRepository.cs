using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the contractor repository.
/// </summary>
public interface IContractorRepository
{
    /// <summary>
    ///     Retrieve all contractors.
    /// </summary>
    IAsyncEnumerable<Contractor> ListAllAsync();
}
