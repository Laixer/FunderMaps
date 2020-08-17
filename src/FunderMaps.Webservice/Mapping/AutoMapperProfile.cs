using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Core.Types.Regions;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Statistics;
using FunderMaps.Webservice.ResponseModels.Types;
using FunderMaps.Webservice.Translation;

namespace FunderMaps.Webservice.Mapping
{
    /// <summary>
    /// Contains an AutoMapper profile for proper DTO mapping.
    /// </summary>
    public sealed class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Sets up our mapping profiles for analysis.
        /// </summary>
        public AutoMapperProfile()
        {
            // All analysis mapping
            CreateMap<AnalysisProduct, AnalysisBuildingDataResponseModel>();
            CreateMap<AnalysisProduct, AnalysisBuildingDescriptionResponseModel>();
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
            CreateMap<Region, RegionResponseModel>();

            // All enums
            CreateMap<AnalysisProductType, AnalysisProductTypeResponseModel>();
            CreateMap<FoundationRisk, FoundationRiskResponseModel>();
            CreateMap<FoundationType, FoundationTypeResponseModel>();
            CreateMap<Reliability, ReliabilityResponseModel>();
            CreateMap<StatisticsProductType, StatisticsProductTypeResponseModel>();
        }
    }
}
