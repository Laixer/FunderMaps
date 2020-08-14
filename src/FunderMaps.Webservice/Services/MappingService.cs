using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Statistics;
using FunderMaps.Webservice.Translation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.Webservice.Services
{
    /// TODO This is too hard coded, this might be optimizable with generics.
    /// TODO Do we really need to add the product to the response?
    /// <summary>
    ///     Manages all our mapping operations.
    /// </summary>
    public sealed class MappingService : IMappingService
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public MappingService(IMapper mapper) => _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        /// <summary>
        /// Map <paramref name="items"/> to a <see cref="ResponseWrapper{AnalysisResponseModelBase}"/>.
        /// </summary>
        /// <remarks>
        /// This explicit mapping is required for our JSON formatter to properly work.
        /// </remarks>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="items"><see cref="AnalysisProduct"/> collection</param>
        /// <returns><see cref="ResponseWrapper{AnalysisResponseModelBase}"/></returns>
        public ResponseWrapper MapToAnalysisWrapper(AnalysisProductType productType, IEnumerable<AnalysisProduct> items)
        {
            if (items == null) { throw new ArgumentNullException(nameof(items)); }

            var wrapper = GetAnalysisWrapper(productType, items);
            //wrapper.Product = ProductTypeTranslator.TranslateAnalysis(productType);
            return wrapper;
        }

        /// <summary>
        /// Map <paramref name="items"/> to a <see cref="ResponseWrapper{StatisticsResponseModelBase}"/>.
        /// </summary>
        /// <remarks>
        /// This explicit mapping is required for our JSON formatter to properly work.
        /// </remarks>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="items"><see cref="StatisticsProduct"/> collection</param>
        /// <returns><see cref="ResponseWrapper{StatisticsResponseModelBase}"/></returns>
        public ResponseWrapper MapToStatisticsWrapper(StatisticsProductType productType, IEnumerable<StatisticsProduct> items)
        {
            if (items == null) { throw new ArgumentNullException(nameof(items)); }

            var wrapper = GetStatisticsWrapper(productType, items);
            //wrapper.Product = ProductTypeTranslator.TranslateStatistics(productType);
            return wrapper;
        }

        /// <summary>
        /// Builds the correct <see cref="ResponseWrapper{AnalysisResponseModelBase}"/>.
        /// </summary>
        /// <param name="productType"><see cref="AnalysisProductType"/></param>
        /// <param name="items"><see cref="AnalysisProduct"/> collection</param>
        /// <returns><see cref="ResponseWrapper{AnalysisResponseModelBase}"/></returns>
        private ResponseWrapper GetAnalysisWrapper(AnalysisProductType productType, IEnumerable<AnalysisProduct> items)
            => productType switch
            {
                // TODO Static lookup array for typeof
                AnalysisProductType.BuildingData => new ResponseWrapper<AnalysisBuildingDataResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<AnalysisProduct, AnalysisBuildingDataResponseModel>(x))
                },
                AnalysisProductType.Foundation => new ResponseWrapper<AnalysisFoundationResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<AnalysisProduct, AnalysisFoundationResponseModel>(x))
                },
                AnalysisProductType.FoundationPlus => new ResponseWrapper<AnalysisFoundationPlusResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<AnalysisProduct, AnalysisFoundationPlusResponseModel>(x))
                },
                AnalysisProductType.Costs => new ResponseWrapper<AnalysisCostsResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<AnalysisProduct, AnalysisCostsResponseModel>(x))
                },
                AnalysisProductType.Complete => new ResponseWrapper<AnalysisCompleteResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<AnalysisProduct, AnalysisCompleteResponseModel>(x)),
                },
                AnalysisProductType.BuildingDescription => new ResponseWrapper<AnalysisBuildingDescriptionResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<AnalysisProduct, AnalysisBuildingDescriptionResponseModel>(x))
                },
                AnalysisProductType.Risk => new ResponseWrapper<AnalysisRiskResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<AnalysisProduct, AnalysisRiskResponseModel>(x))
                },
                _ => throw new InvalidOperationException(nameof(productType)),
            };

        /// <summary>
        /// Builds the correct <see cref="ResponseWrapper{StatisticsResponseModelBase}"/>.
        /// </summary>
        /// <param name="productType"><see cref="StatisticsProductType"/></param>
        /// <param name="items"><see cref="StatisticsProduct"/> collection</param>
        /// <returns><see cref="ResponseWrapper{StatisticsResponseModelBase}"/></returns>
        private ResponseWrapper GetStatisticsWrapper(StatisticsProductType productType, IEnumerable<StatisticsProduct> items) 
            => productType switch
            {
                StatisticsProductType.FoundationRatio => new ResponseWrapper<StatisticsFoundationRatioResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<StatisticsProduct, StatisticsFoundationRatioResponseModel>(x))
                },
                StatisticsProductType.ConstructionYears => new ResponseWrapper<StatisticsConstructionYearsResonseModel>
                {
                    Models = items.Select(x => _mapper.Map<StatisticsProduct, StatisticsConstructionYearsResonseModel>(x))
                },
                StatisticsProductType.FoundationRisk => new ResponseWrapper<StatisticsFoundationRiskResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<StatisticsProduct, StatisticsFoundationRiskResponseModel>(x))
                },
                StatisticsProductType.DataCollected => new ResponseWrapper<StatisticsDataCollectedResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<StatisticsProduct, StatisticsDataCollectedResponseModel>(x))
                },
                StatisticsProductType.BuildingsRestored => new ResponseWrapper<StatisticsBuildingsRestoredResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<StatisticsProduct, StatisticsBuildingsRestoredResponseModel>(x))
                },
                StatisticsProductType.Incidents => new ResponseWrapper<StatisticsIncidentsResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<StatisticsProduct, StatisticsIncidentsResponseModel>(x))
                },
                StatisticsProductType.Reports => new ResponseWrapper<StatisticsReportsResponseModel>
                {
                    Models = items.Select(x => _mapper.Map<StatisticsProduct, StatisticsReportsResponseModel>(x))
                },
                _ => throw new InvalidOperationException(nameof(productType))
            };
    }
}
