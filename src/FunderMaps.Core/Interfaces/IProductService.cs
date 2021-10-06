using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Service to the analysis products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        ///     Get an analysis product.
        /// </summary>
        /// <param name="productType">Product type.</param>
        /// <param name="input">Input query.</param>
        [Obsolete("GetAnalysisAsync is deprecated, please use GetAnalysis2Async instead.")]
        IAsyncEnumerable<AnalysisProduct> GetAnalysisAsync(AnalysisProductType productType, string input);

        /// <summary>
        ///     Get the analysis product.
        /// </summary>
        /// <param name="input">Input query.</param>
        IAsyncEnumerable<AnalysisProduct2> GetAnalysis2Async(string input);

        /// <summary>
        ///     Get statistics per region.
        /// </summary>
        /// <param name="input">Input query.</param>
        IAsyncEnumerable<StatisticsProduct> GetStatisticsAsync(string input);

        /// <summary>
        ///     Get risk index on id.
        /// </summary>
        /// <param name="input">Input query.</param>
        Task<bool> GetRiskIndexAsync(string input);
    }
}
