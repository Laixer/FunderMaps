using FunderMaps.Core.Extensions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Services
{
    /// TODO Duplicate code
    /// <summary>
    /// Manages product requests, including conversion to response models.
    /// </summary>
    public sealed class ProductResultService : IProductResultService
    {
        private readonly IProductService _productService;
        private readonly IMappingService _mappingService;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public ProductResultService(IProductService productService, IMappingService mappingService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
        }

        /// <summary>
        ///     Get an analysis product by bag building id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="bagId">External building bag id.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ResponseWrapper"/></returns>
        public async Task<ResponseWrapper> GetAnalysisByBagIdAsync(Guid userId, AnalysisProductType productType, string bagId, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            bagId.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Get result, map and return.
            var result = await _productService.GetAnalysisByExternalIdAsync(userId, productType, bagId, ExternalDataSource.NlBag, navigation, token).ConfigureAwait(false);
            return _mappingService.MapToAnalysisWrapper(productType, new List<AnalysisProduct> { result });
        }

        /// <summary>
        ///     Get an analysis product by internal building id.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="id">Internal building id.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ResponseWrapper"/></returns>
        public async Task<ResponseWrapper> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            id.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null) { throw new ArgumentNullException(nameof(token)); }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Get result, map and return.
            var result = await _productService.GetAnalysisByIdAsync(userId, productType, id, navigation, token).ConfigureAwait(false);
            return _mappingService.MapToAnalysisWrapper(productType, new List<AnalysisProduct> { result });
        }

        /// <summary>
        ///     Get analysis products based on a query string.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="query">Query string.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ResponseWrapper"/></returns>
        public async Task<ResponseWrapper> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            query.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Get result, map and return.
            var result = await _productService.GetAnalysisByQueryAsync(userId, productType, query, navigation, token).ConfigureAwait(false);
            return _mappingService.MapToAnalysisWrapper(productType, result);
        }

        /// <summary>
        ///     Scrapped for now.
        /// </summary>
        public Task<ResponseWrapper> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation, CancellationToken token) => throw new NotImplementedException();

        /// <summary>
        ///     Gets a statistics product based on a neighborhood code.
        /// </summary>
        /// <param name="userId">Internal user id.</param>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="neighborhoodCode">External neighborhood code.</param>
        /// <param name="navigation"><see cref="INavigation"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="StatisticsResponseWrapper{StatisticsResponseModelBase}"/></returns>
        public async Task<ResponseWrapper> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, string neighborhoodCode, INavigation navigation, CancellationToken token)
        {
            // Validate parameters.
            userId.ThrowIfNullOrEmpty();
            neighborhoodCode.ThrowIfNullOrEmpty();
            navigation.Validate();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Get result, map and return.
            var result = await _productService.GetStatisticsByAreaAsync(userId, productType, neighborhoodCode, navigation, token).ConfigureAwait(false);
            return _mappingService.MapToStatisticsWrapper(productType, new List<StatisticsProduct> { result });
        }

        public Task<ResponseWrapper> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation, CancellationToken token) => throw new NotImplementedException();
    }
}
