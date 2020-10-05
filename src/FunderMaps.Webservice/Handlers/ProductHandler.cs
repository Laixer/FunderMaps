using AutoMapper;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.InputModels;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Statistics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Handlers
{
    /// <summary>
    ///     Handles product requests.
    /// </summary>
    public sealed class ProductHandler
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProductHandler(IMapper mapper, IProductService productService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        private async Task<ResponseWrapper> AsResponseWrapperAsync<TResponseModel, TSource>(IAsyncEnumerable<TSource> itemList)
            where TResponseModel : ResponseModelBase
            where TSource : ProductBase
            => new ResponseWrapper<TResponseModel>
            {
                Models = await _mapper.MapAsync<IList<TResponseModel>, TSource>(itemList)
            };

        // FUTURE Static lookup array for typeof
        /// <summary>
        ///     Builds the correct <see cref="ResponseWrapper{AnalysisResponseModelBase}"/>.
        /// </summary>
        private async Task<ResponseWrapper> AnalysisWrapperFactoryAsync(AnalysisProductType productType, IAsyncEnumerable<AnalysisProduct> itemList)
            => productType switch
            {
                AnalysisProductType.BuildingData => await AsResponseWrapperAsync<AnalysisBuildingDataResponseModel, AnalysisProduct>(itemList),
                AnalysisProductType.Foundation => await AsResponseWrapperAsync<AnalysisFoundationResponseModel, AnalysisProduct>(itemList),
                AnalysisProductType.FoundationPlus => await AsResponseWrapperAsync<AnalysisFoundationPlusResponseModel, AnalysisProduct>(itemList),
                AnalysisProductType.Costs => await AsResponseWrapperAsync<AnalysisCostsResponseModel, AnalysisProduct>(itemList),
                AnalysisProductType.Complete => await AsResponseWrapperAsync<AnalysisCompleteResponseModel, AnalysisProduct>(itemList),
                AnalysisProductType.Risk => await AsResponseWrapperAsync<AnalysisRiskResponseModel, AnalysisProduct>(itemList),
                _ => throw new InvalidOperationException(nameof(productType)),
            };

        /// <summary>
        ///     Builds the correct <see cref="ResponseWrapper{StatisticsResponseModelBase}"/>.
        /// </summary>
        private async Task<ResponseWrapper> StatisticsWrapperFactoryAsync(StatisticsProductType productType, IAsyncEnumerable<StatisticsProduct> itemList)
            => productType switch
            {
                StatisticsProductType.FoundationRatio => await AsResponseWrapperAsync<StatisticsFoundationRatioResponseModel, StatisticsProduct>(itemList),
                StatisticsProductType.ConstructionYears => await AsResponseWrapperAsync<StatisticsConstructionYearsResonseModel, StatisticsProduct>(itemList),
                StatisticsProductType.FoundationRisk => await AsResponseWrapperAsync<StatisticsFoundationRiskResponseModel, StatisticsProduct>(itemList),
                StatisticsProductType.DataCollected => await AsResponseWrapperAsync<StatisticsDataCollectedResponseModel, StatisticsProduct>(itemList),
                StatisticsProductType.BuildingsRestored => await AsResponseWrapperAsync<StatisticsBuildingsRestoredResponseModel, StatisticsProduct>(itemList),
                StatisticsProductType.Incidents => await AsResponseWrapperAsync<StatisticsIncidentsResponseModel, StatisticsProduct>(itemList),
                StatisticsProductType.Reports => await AsResponseWrapperAsync<StatisticsReportsResponseModel, StatisticsProduct>(itemList),
                _ => throw new InvalidOperationException(nameof(productType))
            };

        /// <summary>
        ///     Processes an analysis request.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        internal async Task<ResponseWrapper> ProcessAnalysisRequestAsync(Guid userId, AnalysisInputModel inputModel)
        {
            AnalysisProductType productType = inputModel.Product ?? throw new InvalidProductRequestException(nameof(inputModel.Product));

            // Process according to specified parameters
            if (!string.IsNullOrEmpty(inputModel.Query))
            {
                IAsyncEnumerable<AnalysisProduct> productList = _productService.GetAnalysisByQueryAsync(userId, productType, inputModel.Query, inputModel.Navigation);
                return await AnalysisWrapperFactoryAsync(productType, productList);
            }
            else if (!string.IsNullOrEmpty(inputModel.Id))
            {
                AnalysisProduct product = await _productService.GetAnalysisByIdAsync(userId, productType, inputModel.Id);
                return await AnalysisWrapperFactoryAsync(productType, AsyncEnumerableHelper.AsEnumerable(product));
            }
            else if (!string.IsNullOrEmpty(inputModel.BagId))
            {
                AnalysisProduct product = await _productService.GetAnalysisByExternalIdAsync(userId, productType, inputModel.BagId, ExternalDataSource.NlBag);
                return await AnalysisWrapperFactoryAsync(productType, AsyncEnumerableHelper.AsEnumerable(product));
            }

            // If we reach this point we can't process the request.
            throw new InvalidProductRequestException(nameof(inputModel.Product));
        }

        /// <summary>
        ///     Processes a statistics request.
        /// </summary>
        /// <param name="userId">Internal user id</param>
        internal async Task<ResponseWrapper> ProcessStatisticsRequestAsync(Guid userId, StatisticsInputModel inputModel)
        {
            StatisticsProductType productType = inputModel.Product ?? throw new InvalidProductRequestException(nameof(inputModel.Product));

            // Process according to specified parameters
            if (!string.IsNullOrEmpty(inputModel.NeighborhoodCode))
            {
                StatisticsProduct product = await _productService.GetStatisticsByNeighborhoodAsync(userId, productType, inputModel.NeighborhoodCode);
                return await StatisticsWrapperFactoryAsync(productType, AsyncEnumerableHelper.AsEnumerable(product));
            }

            // If we reach this point we can't process the request.
            throw new InvalidProductRequestException(nameof(inputModel.Product));
        }
    }
}
