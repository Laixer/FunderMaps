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

        public Task<ResponseWrapper> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, INavigation navigation, CancellationToken token) => throw new NotImplementedException();
        public Task<ResponseWrapper> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, INavigation navigation, CancellationToken token) => throw new NotImplementedException();
        public Task<ResponseWrapper> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, INavigation navigation, CancellationToken token) => throw new NotImplementedException();
        public Task<ResponseWrapper> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, string areaCode, INavigation navigation, CancellationToken token) => throw new NotImplementedException();
        public Task<ResponseWrapper> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, INavigation navigation, CancellationToken token) => throw new NotImplementedException();
    }
}
