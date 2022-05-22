using FunderMaps.Core.Types.Products;

namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Service to the analysis products.
/// </summary>
public interface IProductService
{
    /// <summary>
    ///     Get the analysis product.
    /// </summary>
    /// <param name="input">Input query.</param>
    Task<AnalysisProduct3> GetAnalysis3Async(string input);

    /// <summary>
    ///     Get risk index on id.
    /// </summary>
    /// <param name="input">Input query.</param>
    Task<bool> GetRiskIndexAsync(string input);

    /// <summary>
    ///     Get statistics per region.
    /// </summary>
    /// <param name="input">Input query.</param>
    Task<StatisticsProduct> GetStatistics3Async(string input);
}
