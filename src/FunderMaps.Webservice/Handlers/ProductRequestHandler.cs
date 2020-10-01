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
        internal async Task<ResponseWrapper> ProcessAnalysisRequestAsync(Guid userId, AnalysisInputModel inputModel, CancellationToken token)
        {
            // Map product type.
            AnalysisProductType productType = ProductTypeMapper.MapAnalysis(inputModel.Product
                ?? throw new InvalidOperationException(nameof(inputModel.Product)));

            // Process according to specified parameters
            if (!string.IsNullOrEmpty(inputModel.Query))
            {
                IEnumerable<AnalysisProduct> product = await _productService.GetAnalysisByQueryAsync(userId, productType, inputModel.Query, inputModel.Navigation, token);
                return _mappingService.MapToAnalysisWrapper(productType, product);
            }
            else if (!string.IsNullOrEmpty(inputModel.Id))
            {
                AnalysisProduct product = await _productService.GetAnalysisByIdAsync(userId, productType, inputModel.Id, token);
                return _mappingService.MapToAnalysisWrapper(productType, new List<AnalysisProduct> { product });
            }
            else if (!string.IsNullOrEmpty(inputModel.BagId))
            {
                AnalysisProduct product = await _productService.GetAnalysisByExternalIdAsync(userId, productType, inputModel.BagId, ExternalDataSource.NlBag, token);
                return _mappingService.MapToAnalysisWrapper(productType, new List<AnalysisProduct> { product });
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
        internal async Task<ResponseWrapper> ProcessStatisticsRequestAsync(Guid userId, StatisticsInputModel inputModel, CancellationToken token)
        {
            // Map product type.
            StatisticsProductType productType = ProductTypeMapper.MapStatistics(inputModel.Product
                ?? throw new InvalidOperationException(nameof(inputModel.Product)));

            // Process according to specified parameters
            if (!string.IsNullOrEmpty(inputModel.NeighborhoodCode))
            {
                StatisticsProduct product = await _productService.GetStatisticsByNeighborhoodAsync(userId, productType, inputModel.NeighborhoodCode, token);
                return _mappingService.MapToStatisticsWrapper(productType, new List<StatisticsProduct> { product });
            }

            // If we reach this point we can't process the request.
            throw new InvalidProductRequestException(nameof(inputModel.Product));
        }
    }
}
