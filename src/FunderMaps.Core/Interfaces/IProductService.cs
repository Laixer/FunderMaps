using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contract for retrieving products from the data store.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        ///     Gets an analysis product by internal building id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="id">Internal building id.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        Task<AnalysisProduct> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, INavigation navigation, CancellationToken token = default);

        /// <summary>
        ///  Gets an analysis product by external building id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="externalId">External building id.</param>
        /// <param name="externalSource">External data source.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        Task<AnalysisProduct> GetAnalysisByExternalIdAsync(Guid userId, AnalysisProductType productType, string externalId, ExternalDataSource externalSource, INavigation navigation, CancellationToken token = default);

        /// <summary>
        ///     Gets a collection of analysis products by a query.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="query">Query string to search by.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IEnumerable{AnalysisProduct}"/></returns>
        Task<IEnumerable<AnalysisProduct>> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, INavigation navigation, CancellationToken token = default);

        /// <summary>
        ///     Gets all analysis products in a users geofence.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IEnumerable{AnalysisProduct}"/></returns>
        Task<IEnumerable<AnalysisProduct>> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation, CancellationToken token = default);

        /// <summary>
        ///     Gets a statistics product by neighborhood code.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsProduct"/></returns>
        Task<StatisticsProduct> GetStatisticsByNeighborhoodAsync(Guid userId, StatisticsProductType productType, string neighborhoodCode, INavigation navigation, CancellationToken token = default);

        /// <summary>
        ///     Gets all statistics products in a users geofence.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsProduct"/></returns>
        Task<StatisticsProduct> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation, CancellationToken token = default);
    }
}
