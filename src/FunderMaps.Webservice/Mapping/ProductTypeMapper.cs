using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using System;

namespace FunderMaps.Webservice.Mapping
{
    /// <summary>
    /// Contains mapping functionality between <see cref="ProductType"/>s 
    /// and their respective names.
    /// TODO How to do this elegantly? Also see <see cref="Translation.ProductTypeTranslator"/>.
    /// </summary>
    internal static class ProductTypeMapper
    {
        /// <summary>
        /// Maps a given <paramref name="name"/> to the corresponding <see cref="ProductType"/>.
        /// </summary>
        /// <remarks>
        /// This changes the <paramref name="name"/> to lower case before comparing.
        /// </remarks>
        /// <param name="name"><see cref="Product"/> name</param>
        /// <returns><see cref="ProductType"/></returns>
        public static AnalysisProductType MapAnalysisFromString(string name)
        {
            name.ThrowIfNullOrEmpty();
            return name.ToUpperInvariant() switch
            {
                "BUILDINGDATA" => AnalysisProductType.BuildingData,
                "COSTS" => AnalysisProductType.Costs,
                "DESCRIPTION" => AnalysisProductType.BuildingDescription,
                "FOUNDATION" => AnalysisProductType.Foundation,
                "FOUNDATIONPLUS" => AnalysisProductType.FoundationPlus,
                "COMPLETE" => AnalysisProductType.Complete,
                "RISK" => AnalysisProductType.Risk,
                _ => throw new ProductNotFoundException(name),
            };
        }

        /// <summary>
        /// Maps a given <paramref name="name"/> to the corresponding <see cref="ProductType"/>.
        /// </summary>
        /// <remarks>
        /// This changes the <paramref name="name"/> to lower case before comparing.
        /// </remarks>
        /// <param name="name"><see cref="Product"/> name</param>
        /// <returns><see cref="StatisticsProductType"/></returns>
        public static StatisticsProductType MapStatisticsFromString(string name)
        {
            name.ThrowIfNullOrEmpty();
            return name.ToUpperInvariant() switch
            {
                "RESTORATION" => StatisticsProductType.BuildingsRestored,
                "INCIDENTS" => StatisticsProductType.Incidents,
                "REPORTS" => StatisticsProductType.Reports,
                "CONSTRUCTIONYEARS" => StatisticsProductType.ConstructionYears,
                "DATACOLLECTED" => StatisticsProductType.DataCollected,
                "FOUNDATIONRATIO" => StatisticsProductType.FoundationRatio,
                "FOUNDATIONRISK" => StatisticsProductType.FoundationRisk,
                _ => throw new ProductNotFoundException(name),
            };
        }

        /// <summary>
        /// Maps an <see cref="AnalysisProductType"/> to the corresponding
        /// <see cref="AnalysisResponseModelBase"/> implementation.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProductType"/></param>
        /// <returns><see cref="AnalysisResponseModelBase"/> <see cref="Type"/></returns>
        public static Type MapAnalysisResponseModelType(AnalysisProductType product)
        {
            return product switch
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
        }
    }
}
