using FunderMaps.Core.Types.Products;
using System;

namespace FunderMaps.Webservice.Translation
{
    /// <summary>
    /// Translates <see cref="ProductType"/> enums.
    /// TODO Shouldn't this be removed?
    /// TODO This should be done using constants, naming can be cleaned up throughout the project.
    /// </summary>
    internal static class ProductTypeTranslator
    {
        /// <summary>
        /// Translates a <see cref="AnalysisProductType"/> to a human readable form.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProductType"/></param>
        /// <returns>Human readable form of <paramref name="product"/></returns>
        public static string TranslateAnalysis(AnalysisProductType product) 
            => product switch
            {
                AnalysisProductType.BuildingData => "buildingdata",
                AnalysisProductType.Foundation => "foundation",
                AnalysisProductType.FoundationPlus => "foundationplus",
                AnalysisProductType.Costs => "costs",
                AnalysisProductType.Complete => "complete",
                AnalysisProductType.BuildingDescription => "description",
                AnalysisProductType.Risk => "risk",
                _ => throw new InvalidOperationException(nameof(product)),
            };

        /// <summary>
        /// Translates a <see cref="StatisticsProductType"/> to a human readable form.
        /// </summary>
        /// <param name="product"><see cref="StatisticsProductType"/></param>
        /// <returns>Human readable form of <paramref name="product"/></returns>
        public static string TranslateStatistics(StatisticsProductType product)
            => product switch
            {
                StatisticsProductType.FoundationRatio => "foundationratio",
                StatisticsProductType.ConstructionYears => "constructionyears",
                StatisticsProductType.FoundationRisk => "foundationrisk",
                StatisticsProductType.DataCollected => "datacollected",
                StatisticsProductType.BuildingsRestored => "buildingsrestored",
                StatisticsProductType.Incidents => "incidents",
                StatisticsProductType.Reports => "reports",
                _ => throw new InvalidOperationException(nameof(product))
            };
    }
}