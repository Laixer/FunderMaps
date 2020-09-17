using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Types;
using System;

namespace FunderMaps.Webservice.Mapping
{
    /// <summary>
    /// Contains mapping functionality between <see cref="ProductType"/>s 
    /// and their respective core enums.
    /// </summary>
    public static class ProductTypeMapper
    {
        /// <summary>
        ///     Maps an <see cref="AnalysisProductTypeResponseModel"/> to its 
        ///     corresponding <see cref="AnalysisProductType"/>.
        /// </summary>
        /// <param name="input"><see cref="AnalysisProductTypeResponseModel"/></param>
        /// <returns><see cref="AnalysisProductType"/></returns>
        public static AnalysisProductType MapAnalysis(AnalysisProductTypeResponseModel input)
            => input switch
            {
                AnalysisProductTypeResponseModel.BuildingData => AnalysisProductType.BuildingData,
                AnalysisProductTypeResponseModel.Foundation => AnalysisProductType.Foundation,
                AnalysisProductTypeResponseModel.FoundationPlus => AnalysisProductType.FoundationPlus,
                AnalysisProductTypeResponseModel.Costs => AnalysisProductType.Costs,
                AnalysisProductTypeResponseModel.Complete => AnalysisProductType.Complete,
                AnalysisProductTypeResponseModel.Risk => AnalysisProductType.Risk,
                _ => throw new InvalidOperationException(nameof(input))
            };

        /// <summary>
        ///     Maps a <see cref="StatisticsProductTypeResponseModel"/> to its
        ///     corresponding <see cref="StatisticsProductType"/>.
        /// </summary>
        /// <param name="input"><see cref="StatisticsProductTypeResponseModel"/></param>
        /// <returns><see cref="StatisticsProductType"/></returns>
        public static StatisticsProductType MapStatistics(StatisticsProductTypeResponseModel input)
            => input switch
            {
                StatisticsProductTypeResponseModel.FoundationRatio => StatisticsProductType.FoundationRatio,
                StatisticsProductTypeResponseModel.ConstructionYears => StatisticsProductType.ConstructionYears,
                StatisticsProductTypeResponseModel.FoundationRisk => StatisticsProductType.FoundationRisk,
                StatisticsProductTypeResponseModel.DataCollected => StatisticsProductType.DataCollected,
                StatisticsProductTypeResponseModel.BuildingsRestored => StatisticsProductType.BuildingsRestored,
                StatisticsProductTypeResponseModel.Incidents => StatisticsProductType.Incidents,
                StatisticsProductTypeResponseModel.Reports => StatisticsProductType.Reports,
                _ => throw new InvalidOperationException(nameof(input))
            };

        /// <summary>
        /// Maps an <see cref="AnalysisProductType"/> to the corresponding
        /// <see cref="AnalysisResponseModelBase"/> implementation.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProductType"/></param>
        /// <returns><see cref="AnalysisResponseModelBase"/> <see cref="Type"/></returns>
        public static Type MapAnalysisResponseModelType(AnalysisProductType product) => product switch
        {
            AnalysisProductType.BuildingData => typeof(AnalysisBuildingDataResponseModel),
            AnalysisProductType.Foundation => typeof(AnalysisFoundationResponseModel),
            AnalysisProductType.FoundationPlus => typeof(AnalysisFoundationPlusResponseModel),
            AnalysisProductType.Costs => typeof(AnalysisCostsResponseModel),
            AnalysisProductType.Complete => typeof(AnalysisCompleteResponseModel),
            AnalysisProductType.Risk => typeof(AnalysisRiskResponseModel),
            _ => throw new InvalidOperationException(nameof(product)),
        };
    }
}
