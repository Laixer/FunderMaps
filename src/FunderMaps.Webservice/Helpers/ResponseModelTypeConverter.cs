using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Statistics;
using System;

namespace FunderMaps.Webservice.Helpers
{
    /// <summary>
    ///     Contains mapping functionality for response model types.
    /// </summary>
    public static class ResponseModelTypeConverter
    {
        /// <summary>
        ///     Maps an <see cref="AnalysisProductType"/> to the corresponding
        ///     <see cref="AnalysisResponseModelBase"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProductType"/></param>
        /// <returns><see cref="AnalysisResponseModelBase"/></returns>
        public static Type MapAnalysis(AnalysisProductType product)
            => product switch
            {
                AnalysisProductType.BuildingData => typeof(AnalysisBuildingDataResponseModel),
                AnalysisProductType.Foundation => typeof(AnalysisFoundationResponseModel),
                AnalysisProductType.FoundationPlus => typeof(AnalysisFoundationPlusResponseModel),
                AnalysisProductType.Costs => typeof(AnalysisCostsResponseModel),
                AnalysisProductType.Complete => typeof(AnalysisCompleteResponseModel),
                AnalysisProductType.BuildingDescription => typeof(AnalysisBuildingDescriptionResponseModel),
                AnalysisProductType.Risk => typeof(AnalysisRiskResponseModel),
                _ => throw new InvalidOperationException(nameof(product)),
            };

        /// <summary>
        ///     Maps a <see cref="StatisticsProductType"/> to the corresponding
        ///     <see cref="StatisticsResponseModelBase"/>.
        /// </summary>
        /// <param name="product"><see cref="StatisticsProductType"/></param>
        /// <returns><see cref="StatisticsResponseModelBase"/></returns>
        public static Type MapStatistics(StatisticsProductType product)
            => product switch
            {
                StatisticsProductType.FoundationRatio => typeof(StatisticsFoundationRatioResponseModel),
                StatisticsProductType.ConstructionYears => typeof(StatisticsConstructionYearsResonseModel),
                StatisticsProductType.FoundationRisk => typeof(StatisticsFoundationRiskResponseModel),
                StatisticsProductType.DataCollected => typeof(StatisticsDataCollectedResponseModel),
                StatisticsProductType.BuildingsRestored => typeof(StatisticsBuildingsRestoredResponseModel),
                StatisticsProductType.Incidents => typeof(StatisticsIncidentsResponseModel),
                StatisticsProductType.Reports => typeof(StatisticsReportsResponseModel),
                _ => throw new InvalidOperationException(nameof(product)),
            };
    }
}
