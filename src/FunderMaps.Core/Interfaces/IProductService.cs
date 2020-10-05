using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    ///     Contract for retrieving products from the data store.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        ///     Gets an analysis product by internal building id.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="productType">Product type.</param>
        /// <param name="id">Internal building id.</param>
        ValueTask<AnalysisProduct> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id);

        /// <summary>
        ///  Gets an analysis product by external building id.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="productType">Product type.</param>
        /// <param name="externalId">External building id.</param>
        /// <param name="externalSource">External data source.</param>
        ValueTask<AnalysisProduct> GetAnalysisByExternalIdAsync(Guid userId, AnalysisProductType productType, string externalId, ExternalDataSource externalSource);

        /// <summary>
        ///     Gets a collection of analysis products by a query.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="productType">Product type.</param>
        /// <param name="query">Query string to search by.</param>
        IAsyncEnumerable<AnalysisProduct> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, INavigation navigation);

        /// <summary>
        ///     Gets all analysis products in a users geofence.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="productType">Product type.</param>
        IAsyncEnumerable<AnalysisProduct> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation);

        /// <summary>
        ///     Gets a statistics product by neighborhood code.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="productType">Product type.</param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        ValueTask<StatisticsProduct> GetStatisticsByNeighborhoodAsync(Guid userId, StatisticsProductType productType, string neighborhoodCode);

        /// <summary>
        ///     Gets all statistics products in a users geofence.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="productType">Product type.</param>
        ValueTask<StatisticsProduct> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation);
    }
}
