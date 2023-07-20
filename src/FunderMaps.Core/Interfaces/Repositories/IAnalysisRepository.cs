using FunderMaps.Core.Types.Products;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the analysis repository.
/// </summary>
public interface IAnalysisRepository
{
    /// <summary>
    ///     Gets an analysis product by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    Task<AnalysisProduct> GetAsync(string id);

    /// <summary>
    ///     Gets the risk index by its internal building id.
    /// </summary>
    /// <param name="id">Internal building id.</param>
    Task<bool> GetRiskIndexAsync(string id);

    /// <summary>
    ///     Register a product match.
    /// </summary>
    /// <param name="buildingId">Internal building id.</param>
    /// <param name="id">External identifier.</param>
    Task<bool> RegisterProductMatch(string buildingId, string id, string product);

    /// <summary>
    ///     Register a product mismatch.
    /// </summary>
    /// <param name="id">External identifier.</param>
    Task RegisterProductMismatch(string id);
}
