using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the contractor repository.
/// </summary>
public interface IBundleRepository : IAsyncRepository<Bundle, string>
{
    /// <summary>
    ///     Retrieve all enabled <see cref="Bundle"/>.
    /// </summary>
    /// <returns>List of <see cref="Bundle"/>.</returns>
    IAsyncEnumerable<Bundle> ListAllEnabledAsync();

    /// <summary>
    ///     Run precondition.
    /// </summary>
    Task<bool> RunPreconditionAsync(string id, string precondition);

    /// <summary>
    ///     Log the built time of a bundle.
    /// </summary>
    Task LogBuiltTimeAsync(string id);
}
