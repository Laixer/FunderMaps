using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    // FUTURE: Rename to TelemetryProductService
    /// <summary>
    ///     Retrieves products and tracks user behaviour.
    /// </summary>
    public class ProductTrackingService : ProductService
    {
        private readonly ITrackingRepository _trackingRepository;

        /// <summary>
        ///     Create new instance and invoke base.
        /// </summary>
        public ProductTrackingService(IAnalysisRepository analysisRepository,
            IStatisticsRepository statisticsRepository,
            ITrackingRepository trackingRepository)
            : base(analysisRepository, statisticsRepository)
                => _trackingRepository = trackingRepository ?? throw new ArgumentNullException(nameof(trackingRepository));

        /// <summary>
        ///     Gets a single analysis based on an external id. Also track product
        ///     usage in the data store.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="externalId">External building id.</param>
        /// <param name="externalSource">External data source.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public override async Task<AnalysisProduct> GetAnalysisByExternalIdAsync(Guid userId, AnalysisProductType productType, string externalId,
            ExternalDataSource externalSource, INavigation navigation, CancellationToken token = default)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            externalId.ThrowIfNullOrEmpty();
            navigation.Validate();

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Perform base call to get the product.
            var result = await base.GetAnalysisByExternalIdAsync(userId, productType, externalId, externalSource, navigation, token);

            // Track usage for user.
            await _trackingRepository.ProcessAnalysisUsageAsync(userId, productType, 1U, token);

            return result;
        }

        /// <summary>
        ///     Gets a single analysis based on an internal id. Also track product
        ///     usage in the data store.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="id">Internal building id.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AnalysisProduct"/></returns>
        public override async Task<AnalysisProduct> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id,
            INavigation navigation, CancellationToken token = default)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            id.ThrowIfNullOrEmpty();
            navigation.Validate();

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Perform base call to get the product.
            var result = await base.GetAnalysisByIdAsync(userId, productType, id, navigation, token);

            // Track usage for user.
            await _trackingRepository.ProcessAnalysisUsageAsync(userId, productType, 1U, token);

            return result;
        }

        /// <summary>
        ///     Gets an analysis collection based on a query string. Also track 
        ///     product usage in the data store.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="query">Query search string.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IEnumerable{AnalysisProduct}"/></returns>
        public override async Task<IEnumerable<AnalysisProduct>> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType,
            string query, INavigation navigation, CancellationToken token = default)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            query.ThrowIfNullOrEmpty();
            navigation.Validate();

            // Check for cancellation.
            token.ThrowIfCancellationRequested();
            // Perform base call to get the product.
            var result = await base.GetAnalysisByQueryAsync(userId, productType, query, navigation, token);

            // Track usage for user.
            await _trackingRepository.ProcessAnalysisUsageAsync(userId, productType, (uint)result.Count(), token);

            return result;
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IEnumerable{AnalysisProduct}"/></returns>
        public override Task<IEnumerable<AnalysisProduct>> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation, CancellationToken token = default)
            => throw new NotImplementedException();

        /// <summary>
        ///     Gets statistics for a given area code. Also track product usage 
        ///     in the data store.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="neighborhoodCode"/> can be a district or municipality
        ///     based on the <paramref name="productType"/>.
        /// </remarks>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="neighborhoodCode">Neighborhood code.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsProduct"/></returns>
        public override async Task<StatisticsProduct> GetStatisticsByNeighborhoodAsync(Guid userId, StatisticsProductType productType, string neighborhoodCode, INavigation navigation, CancellationToken token = default)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            neighborhoodCode.ThrowIfNullOrEmpty();
            navigation.Validate();

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Perform base call to get the product.
            var result = await base.GetStatisticsByNeighborhoodAsync(userId, productType, neighborhoodCode, navigation, token);

            // Track usage for user.
            await _trackingRepository.ProcessStatisticsUsageAsync(userId, productType, 1, token);

            return result;
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsProduct"/></returns>
        public override Task<StatisticsProduct> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation, CancellationToken token = default)
            => throw new NotImplementedException();
    }
}
