using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels;
using System.Collections.Generic;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    ///     Handles our DTO mapping.
    /// </summary>
    public interface IMappingService
    {
        /// <summary>
        ///     Map incoming analysis items to a <see cref="ResponseWrapper"/>.
        /// </summary>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="items"><see cref="AnalysisProduct"/> collection</param>
        /// <returns><see cref="ResponseWrapper"/></returns>
        public ResponseWrapper MapToAnalysisWrapper(AnalysisProductType productType, IEnumerable<AnalysisProduct> items);

        /// <summary>
        ///     Map incoming statistics items to a <see cref="ResponseWrapper"/>.
        /// </summary>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="items"><see cref="StatisticsProduct"/> collection</param>
        /// <returns><see cref="ResponseWrapper"/></returns>
        public ResponseWrapper MapToStatisticsWrapper(StatisticsProductType productType, IEnumerable<StatisticsProduct> items);
    }
}
