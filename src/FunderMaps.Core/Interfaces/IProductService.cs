﻿using FunderMaps.Core.Types.Products;
using System.Collections.Generic;

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
        IAsyncEnumerable<AnalysisProduct> GetAnalysisAsync(AnalysisProductType productType, string input);

        /// <summary>
        ///     Get an analysis product v2.
        /// </summary>
        /// <param name="input">Input query.</param>
        IAsyncEnumerable<AnalysisProduct2> GetAnalysis2Async(string input);

        /// <summary>
        ///     Get statistics per region.
        /// </summary>
        /// <param name="input">Input query.</param>
        IAsyncEnumerable<StatisticsProduct> GetStatisticsAsync(string input);
    }
}
