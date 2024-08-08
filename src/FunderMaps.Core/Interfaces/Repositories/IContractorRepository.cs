using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the contractor repository.
/// </summary>
public interface IContractorRepository : IAsyncRepository<Contractor, int>
{
    /// <summary>
    ///     Retrieve all contractors.
    /// </summary>
    IAsyncEnumerable<Contractor> ListAllAsync();
}
