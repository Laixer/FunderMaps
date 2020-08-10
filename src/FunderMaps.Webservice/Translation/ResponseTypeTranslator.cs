using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using System;

namespace FunderMaps.Webservice.Translation
{
    /// <summary>
    /// Contains translation functionality for response types.
    /// </summary>
    internal static class ResponseTypeTranslator
    {
        /// <summary>
        /// Translates a <see cref="AnalysisResponseModelBase"/> to the corresponding
        /// string value.
        /// </summary>
        /// <remarks>
        /// <see cref="AnalysisFoundationPlusResponseModel"/> has to preceed
        /// <see cref="AnalysisFoundationResponseModel"/> due to inheritance.
        /// </remarks>
        /// <param name="model"><see cref="AnalysisResponseModelBase"/></param>
        /// <returns><see cref="AnalysisResponseModelBase"/> string value</returns>
        public static string TranslateAnalysis<TModelBase>(TModelBase model)
            where TModelBase : AnalysisResponseModelBase
        {
            if (model == null) { throw new ArgumentNullException(nameof(model)); }

            return model switch
            {
                AnalysisBuildingDataResponseModel x => ProductTypeTranslator.TranslateAnalysis(AnalysisProductType.BuildingData),
                AnalysisBuildingDescriptionResponseModel x => ProductTypeTranslator.TranslateAnalysis(AnalysisProductType.BuildingDescription),
                AnalysisCompleteResponseModel x => ProductTypeTranslator.TranslateAnalysis(AnalysisProductType.Complete),
                AnalysisCostsResponseModel x => ProductTypeTranslator.TranslateAnalysis(AnalysisProductType.Costs),
                AnalysisFoundationPlusResponseModel x => ProductTypeTranslator.TranslateAnalysis(AnalysisProductType.Foundation),
                AnalysisFoundationResponseModel x => ProductTypeTranslator.TranslateAnalysis(AnalysisProductType.FoundationPlus),
                AnalysisRiskResponseModel x => ProductTypeTranslator.TranslateAnalysis(AnalysisProductType.Risk),
                _ => throw new InvalidOperationException(nameof(model)),
            };
        }
    }
}
