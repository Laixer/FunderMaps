using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Testing.Faker;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Types;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FunderMaps.Webservice.UnitTests.Mapping
{
    public class AnalysisMappingTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IMappingService _mappingService;
        private readonly List<AnalysisProduct> analysisProducts;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AnalysisMappingTests(WebApplicationFactory<Startup> factory)
        {
            _mappingService = factory.Services.GetService<IMappingService>();
            analysisProducts = new AnalysisProductFaker() // TODO Invalid ref, should be put into different package
                .Generate(10);
            var statisticsProducts = new StatisticsProductFaker()
                .Generate(10);

            // FUTURE This seems too hard coded, enhance.
            for (int i = 0; i < analysisProducts.Count; i++)
            {
                analysisProducts[i].ConstructionYearDistribution = statisticsProducts[i].ConstructionYearDistribution;
                analysisProducts[i].DataCollectedPercentage = statisticsProducts[i].DataCollectedPercentage;
                analysisProducts[i].FoundationRiskDistribution = statisticsProducts[i].FoundationRiskDistribution;
                analysisProducts[i].FoundationTypeDistribution = statisticsProducts[i].FoundationTypeDistribution;
                analysisProducts[i].TotalBuildingRestoredCount = statisticsProducts[i].TotalBuildingRestoredCount;
                analysisProducts[i].TotalIncidentCount = statisticsProducts[i].TotalIncidentCount;
                analysisProducts[i].TotalReportCount = statisticsProducts[i].TotalReportCount;
            }
        }

        [Fact]
        public void MapBuildingData()
        {
            // Act
            var mapped = _mappingService.MapToAnalysisWrapper(AnalysisProductType.BuildingData, new List<AnalysisProduct> { analysisProducts[0] })
                as ResponseWrapper<AnalysisBuildingDataResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(analysisProducts[0].Id, model.Id);
            Assert.Equal(analysisProducts[0].NeighborhoodId, model.NeighborhoodId);
            Assert.Equal(analysisProducts[0].BuildingHeight, model.BuildingHeight);
            Assert.Equal(analysisProducts[0].ConstructionYear, model.ConstructionYear);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationType), model.FoundationType);
        }

        [Fact]
        public void MapFoundation()
        {
            // Act
            var mapped = _mappingService.MapToAnalysisWrapper(AnalysisProductType.Foundation, new List<AnalysisProduct> { analysisProducts[0] })
                as ResponseWrapper<AnalysisFoundationResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(analysisProducts[0].Id, model.Id);
            Assert.Equal(analysisProducts[0].NeighborhoodId, model.NeighborhoodId);
            Assert.Equal(analysisProducts[0].DewateringDepth, model.DewateringDepth);
            Assert.Equal(analysisProducts[0].Drystand, model.Drystand);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationRisk), model.FoundationRisk);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationType), model.FoundationType);
            Assert.Equal(analysisProducts[0].GroundLevel, model.GroundLevel);
            Assert.Equal(analysisProducts[0].GroundWaterLevel, model.GroundWaterLevel);
        }

        [Fact]
        public void MapFoundationPlus()
        {
            // Act
            var mapped = _mappingService.MapToAnalysisWrapper(AnalysisProductType.FoundationPlus, new List<AnalysisProduct> { analysisProducts[0] })
                as ResponseWrapper<AnalysisFoundationPlusResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(analysisProducts[0].Id, model.Id);
            Assert.Equal(analysisProducts[0].NeighborhoodId, model.NeighborhoodId);
            Assert.Equal(analysisProducts[0].DataCollectedPercentage, model.DataCollectedPercentage);
            Assert.Equal(analysisProducts[0].DewateringDepth, model.DewateringDepth);
            Assert.Equal(analysisProducts[0].Drystand, model.Drystand);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationRisk), model.FoundationRisk);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationType), model.FoundationType);
            Assert.Equal(analysisProducts[0].GroundLevel, model.GroundLevel);
            Assert.Equal(analysisProducts[0].GroundWaterLevel, model.GroundWaterLevel);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].Reliability), model.Reliability);
            Assert.Equal(analysisProducts[0].TotalReportCount, model.TotalReportCount);

            ValidateConstructionYearDistribution(analysisProducts[0].ConstructionYearDistribution, model.ConstructionYearDistribution);
            ValidateFoundationRiskDistribution(analysisProducts[0].FoundationRiskDistribution, model.FoundationRiskDistribution);
            ValidateFoundationTypeDistribution(analysisProducts[0].FoundationTypeDistribution, model.FoundationTypeDistribution);
        }

        [Fact]
        public void MapCosts()
        {
            // Act
            var mapped = _mappingService.MapToAnalysisWrapper(AnalysisProductType.Costs, new List<AnalysisProduct> { analysisProducts[0] })
                as ResponseWrapper<AnalysisCostsResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(analysisProducts[0].Id, model.Id);
            Assert.Equal(analysisProducts[0].NeighborhoodId, model.NeighborhoodId);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationRisk), model.FoundationRisk);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].Reliability), model.Reliability);
            Assert.Equal(analysisProducts[0].RestorationCosts, model.RestorationCosts);
            Assert.Equal(analysisProducts[0].TotalBuildingRestoredCount, model.TotalBuildingRestoredCount);
            Assert.Equal(analysisProducts[0].TotalIncidentCount, model.TotalIncidentCount);
        }

        [Fact]
        public void MapComplete()
        {
            // Act
            var mapped = _mappingService.MapToAnalysisWrapper(AnalysisProductType.Complete, new List<AnalysisProduct> { analysisProducts[0] })
                as ResponseWrapper<AnalysisCompleteResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(analysisProducts[0].Id, model.Id);
            Assert.Equal(analysisProducts[0].NeighborhoodId, model.NeighborhoodId);
            Assert.Equal(analysisProducts[0].BuildingHeight, model.BuildingHeight);
            Assert.Equal(analysisProducts[0].ConstructionYear, model.ConstructionYear);
            Assert.Equal(analysisProducts[0].DataCollectedPercentage, model.DataCollectedPercentage);
            Assert.Equal(analysisProducts[0].DewateringDepth, model.DewateringDepth);
            Assert.Equal(analysisProducts[0].Drystand, model.Drystand);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationRisk), model.FoundationRisk);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationType), model.FoundationType);
            Assert.Equal(analysisProducts[0].GroundLevel, model.GroundLevel);
            Assert.Equal(analysisProducts[0].GroundWaterLevel, model.GroundWaterLevel);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].Reliability), model.Reliability);
            Assert.Equal(analysisProducts[0].RestorationCosts, model.RestorationCosts);
            Assert.Equal(analysisProducts[0].TotalBuildingRestoredCount, model.TotalBuildingRestoredCount);
            Assert.Equal(analysisProducts[0].TotalIncidentCount, model.TotalIncidentCount);
            Assert.Equal(analysisProducts[0].TotalReportCount, model.TotalReportCount);

            ValidateConstructionYearDistribution(analysisProducts[0].ConstructionYearDistribution, model.ConstructionYearDistribution);
            ValidateFoundationRiskDistribution(analysisProducts[0].FoundationRiskDistribution, model.FoundationRiskDistribution);
            ValidateFoundationTypeDistribution(analysisProducts[0].FoundationTypeDistribution, model.FoundationTypeDistribution);
        }

        [Fact]
        public void MapRisk()
        {
            // Act
            var mapped = _mappingService.MapToAnalysisWrapper(AnalysisProductType.Risk, new List<AnalysisProduct> { analysisProducts[0] })
                as ResponseWrapper<AnalysisRiskResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(analysisProducts[0].Id, model.Id);
            Assert.Equal(analysisProducts[0].NeighborhoodId, model.NeighborhoodId);
            Assert.Equal(analysisProducts[0].DewateringDepth, model.DewateringDepth);
            Assert.Equal(analysisProducts[0].Drystand, model.Drystand);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationRisk), model.FoundationRisk);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].FoundationType), model.FoundationType);
            Assert.Equal(EnumMapperHelper.Map(analysisProducts[0].Reliability), model.Reliability);
            Assert.Equal(analysisProducts[0].RestorationCosts, model.RestorationCosts);
        }

        // FUTURE: Don't re-use this?
        /// <summary>
        ///     Validate a <see cref="ConstructionYearDistributionResponseModel"/>.
        /// </summary>
        /// <param name="entity"><see cref="ConstructionYearDistribution"/></param>
        /// <param name="model"><see cref="ConstructionYearDistributionResponseModel"/></param>
        internal static void ValidateConstructionYearDistribution(ConstructionYearDistribution entity, ConstructionYearDistributionResponseModel model)
        {
            // If both are null we are good to go.
            if (entity == null && model == null)
            {
                return;
            }

            // Assert.
            Assert.Equal(entity.Decades.Count(), model.Decades.Count());
            for (int i = 0; i < entity.Decades.Count(); i++)
            {
                var entityPair = entity.Decades.ToArray()[i];
                var modelPair = model.Decades.ToArray()[i];
                Assert.Equal(entityPair.Decade.YearFrom, modelPair.Decade.YearFrom);
                Assert.Equal(entityPair.Decade.YearTo, modelPair.Decade.YearTo);
                Assert.Equal(entityPair.TotalCount, modelPair.TotalCount);
            }
        }

        // FUTURE: Don't re-use this?
        /// <summary>
        ///     Validate a <see cref="FoundationRiskDistributionResponseModel"/>.
        /// </summary>
        /// <param name="entity"><see cref="FoundationRiskDistribution"/></param>
        /// <param name="model"><see cref="FoundationRiskDistributionResponseModel"/></param>
        internal static void ValidateFoundationRiskDistribution(FoundationRiskDistribution entity, FoundationRiskDistributionResponseModel model)
        {
            // If both are null we are good to go.
            if (entity == null && model == null)
            {
                return;
            }

            // Assert.
            Assert.Equal(entity.PercentageA, model.PercentageA);
            Assert.Equal(entity.PercentageB, model.PercentageB);
            Assert.Equal(entity.PercentageC, model.PercentageC);
            Assert.Equal(entity.PercentageD, model.PercentageD);
            Assert.Equal(entity.PercentageE, model.PercentageE);
        }

        // FUTURE: Don't re-use this?
        /// <summary>
        ///     Validate a <see cref="FoundationTypeDistributionResponseModel"/>.
        /// </summary>
        /// <param name="entity"><see cref="FoundationTypeDistribution"/></param>
        /// <param name="model"><see cref="FoundationTypeDistributionResponseModel"/></param>
        internal static void ValidateFoundationTypeDistribution(FoundationTypeDistribution entity, FoundationTypeDistributionResponseModel model)
        {
            // If both are null we are good to go.
            if (entity == null && model == null)
            {
                return;
            }

            // Assert.
            Assert.Equal(entity.FoundationTypes.Count(), model.FoundationTypes.Count());

            for (int i = 0; i < entity.FoundationTypes.Count(); i++)
            {
                var entityPair = entity.FoundationTypes.ToArray()[i];
                var modelPair = model.FoundationTypes.ToArray()[i];
                Assert.Equal(EnumMapperHelper.Map(entityPair.FoundationType), modelPair.FoundationType);
                Assert.Equal(entityPair.Percentage, modelPair.Percentage);
            }
        }
    }
}
