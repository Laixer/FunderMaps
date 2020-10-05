using AutoMapper;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Statistics;
using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice
{
    /// <summary>
    ///     Contains an AutoMapper profile for proper DTO mapping.
    /// </summary>
    public sealed class MapperProfile : Profile
    {
        /// <summary>
        ///     Sets up our mapping profiles for analysis.
        /// </summary>
        public MapperProfile()
        {
            // All analysis mapping
            CreateMap<AnalysisProduct, AnalysisBuildingDataResponseModel>();
            CreateMap<AnalysisProduct, AnalysisCompleteResponseModel>();
            CreateMap<AnalysisProduct, AnalysisCostsResponseModel>();
            CreateMap<AnalysisProduct, AnalysisFoundationPlusResponseModel>();
            CreateMap<AnalysisProduct, AnalysisFoundationResponseModel>();
            CreateMap<AnalysisProduct, AnalysisRiskResponseModel>();

            // All statistics mapping
            CreateMap<StatisticsProduct, StatisticsBuildingsRestoredResponseModel>();
            CreateMap<StatisticsProduct, StatisticsConstructionYearsResonseModel>();
            CreateMap<StatisticsProduct, StatisticsDataCollectedResponseModel>();
            CreateMap<StatisticsProduct, StatisticsFoundationRatioResponseModel>();
            CreateMap<StatisticsProduct, StatisticsFoundationRiskResponseModel>();
            CreateMap<StatisticsProduct, StatisticsIncidentsResponseModel>();
            CreateMap<StatisticsProduct, StatisticsReportsResponseModel>();

            // All custom types we use
            CreateMap<Years, YearsResponseModel>();
            CreateMap<ConstructionYearDistribution, ConstructionYearDistributionResponseModel>();
            CreateMap<ConstructionYearPair, ConstructionYearPairResponseModel>();
            CreateMap<FoundationRiskDistribution, FoundationRiskDistributionResponseModel>();
            CreateMap<FoundationTypeDistribution, FoundationTypeDistributionResponseModel>();
            CreateMap<FoundationTypePair, FoundationTypePairResponseModel>();
        }
    }
}
