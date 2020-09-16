using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.InputModels;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Handlers
{
    /// <summary>
    ///     Handles product requests.
    /// </summary>
    public sealed class ProductRequestHandler
    {
        private readonly IProductService _productService;
        private readonly IMappingService _mappingService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProductRequestHandler(IProductService productService, IMappingService mappingService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
        }

        /// <summary>
        ///     Processes an analysis request.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        /// <param name="inputModel"><see cref="AnalysisInputModel"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ResponseWrapper{AnalysisResponseModelBase}"/></returns>
        internal async Task<ResponseWrapper> ProcessAnalysisRequestAsync(Guid userId, AnalysisInputModel inputModel, CancellationToken token = default)
        {
            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Map product type.
            AnalysisProductType product = ProductTypeMapper.MapAnalysis(inputModel.Product
                ?? throw new InvalidOperationException(nameof(inputModel.Product)));

            // Process according to specified parameters
            if (!string.IsNullOrEmpty(inputModel.Query))
            {
                var result = await _productService.GetAnalysisByQueryAsync(userId, product, inputModel.Query, inputModel.Navigation, token);
                return _mappingService.MapToAnalysisWrapper(product, result);
            }
            else if (!string.IsNullOrEmpty(inputModel.Id))
            {
                var result = await _productService.GetAnalysisByIdAsync(userId, product, inputModel.Id, inputModel.Navigation, token);
                return _mappingService.MapToAnalysisWrapper(product, new List<AnalysisProduct> { result });
            }
            else if (!string.IsNullOrEmpty(inputModel.BagId))
            {
                var result = await _productService.GetAnalysisByExternalIdAsync(userId, product, inputModel.BagId, ExternalDataSource.NlBag, inputModel.Navigation, token);
                return _mappingService.MapToAnalysisWrapper(product, new List<AnalysisProduct> { result });
            }

            // If we reach this point we can't process the request.
            throw new InvalidProductRequestException(nameof(inputModel.Product));
        }

        /// <summary>
        ///     Processes a statistics request.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        /// <param name="inputModel"><see cref="StatisticsInputModel"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="ResponseWrapper{StatisticsResponseModelBase}"/></returns>
        internal async Task<ResponseWrapper> ProcessStatisticsRequestAsync(Guid userId, StatisticsInputModel inputModel, CancellationToken token = default)
        {
            // Check for cancellation.
            token.ThrowIfCancellationRequested();

            // Map product type.
            StatisticsProductType product = ProductTypeMapper.MapStatistics(inputModel.Product
                ?? throw new InvalidOperationException(nameof(inputModel.Product)));

            // Process according to specified parameters
            if (!string.IsNullOrEmpty(inputModel.NeighborhoodCode))
            {
                var result = await _productService.GetStatisticsByNeighborhoodAsync(userId, product, inputModel.NeighborhoodCode, inputModel.Navigation, token);
                return _mappingService.MapToStatisticsWrapper(product, new List<StatisticsProduct> { result });
            }

            // If we reach this point we can't process the request.
            throw new InvalidProductRequestException(nameof(inputModel.Product));
        }
    }
}
