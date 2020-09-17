using FunderMaps.Core.Types.Products;
using FunderMaps.Testing.Faker;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Statistics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FunderMaps.Webservice.UnitTests.Mapping
{
    public class StatisticsMappingTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IMappingService _mappingService;
        private readonly StatisticsProduct statisticsProduct;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StatisticsMappingTests(WebApplicationFactory<Startup> factory)
        {
            _mappingService = factory.Services.GetService<IMappingService>();
            statisticsProduct = new StatisticsProductFaker().Generate();
        }

        [Fact]
        public void MapFoundationRatio()
        {
            // Act
            var mapped = _mappingService.MapToStatisticsWrapper(StatisticsProductType.FoundationRatio, new List<StatisticsProduct> { statisticsProduct })
                as ResponseWrapper<StatisticsFoundationRatioResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(statisticsProduct.FoundationTypeDistribution.FoundationTypes.Count(), model.FoundationTypeDistribution.FoundationTypes.Count());

            for (int i = 0; i < statisticsProduct.FoundationTypeDistribution.FoundationTypes.Count(); i++)
            {
                var entityPair = statisticsProduct.FoundationTypeDistribution.FoundationTypes.ToArray()[i];
                var modelPair = model.FoundationTypeDistribution.FoundationTypes.ToArray()[i];
                Assert.Equal(EnumMapperHelper.Map(entityPair.FoundationType), modelPair.FoundationType);
                Assert.Equal(entityPair.Percentage, modelPair.Percentage);
            }
        }

        [Fact]
        public void MapConstructionYears()
        {
            // Act
            var mapped = _mappingService.MapToStatisticsWrapper(StatisticsProductType.ConstructionYears, new List<StatisticsProduct> { statisticsProduct })
                as ResponseWrapper<StatisticsConstructionYearsResonseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(statisticsProduct.ConstructionYearDistribution.Decades.Count(), model.ConstructionYearDistribution.Decades.Count());
            for (int i = 0; i < statisticsProduct.ConstructionYearDistribution.Decades.Count(); i++)
            {
                var entityPair = statisticsProduct.ConstructionYearDistribution.Decades.ToArray()[i];
                var modelPair = model.ConstructionYearDistribution.Decades.ToArray()[i];
                Assert.Equal(entityPair.Decade.YearFrom, modelPair.Decade.YearFrom);
                Assert.Equal(entityPair.Decade.YearTo, modelPair.Decade.YearTo);
                Assert.Equal(entityPair.TotalCount, modelPair.TotalCount);
            }
        }

        [Fact]
        public void MapFoundationRisk()
        {
            // Act
            var mapped = _mappingService.MapToStatisticsWrapper(StatisticsProductType.FoundationRisk, new List<StatisticsProduct> { statisticsProduct })
                as ResponseWrapper<StatisticsFoundationRiskResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(statisticsProduct.FoundationRiskDistribution.PercentageA, model.FoundationRiskDistribution.PercentageA);
            Assert.Equal(statisticsProduct.FoundationRiskDistribution.PercentageB, model.FoundationRiskDistribution.PercentageB);
            Assert.Equal(statisticsProduct.FoundationRiskDistribution.PercentageC, model.FoundationRiskDistribution.PercentageC);
            Assert.Equal(statisticsProduct.FoundationRiskDistribution.PercentageD, model.FoundationRiskDistribution.PercentageD);
            Assert.Equal(statisticsProduct.FoundationRiskDistribution.PercentageE, model.FoundationRiskDistribution.PercentageE);
        }

        [Fact]
        public void MapDataCollected()
        {
            // Act
            var mapped = _mappingService.MapToStatisticsWrapper(StatisticsProductType.DataCollected, new List<StatisticsProduct> { statisticsProduct })
                as ResponseWrapper<StatisticsDataCollectedResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(statisticsProduct.DataCollectedPercentage, model.DataCollectedPercentage);
        }

        [Fact]
        public void MapBuildingsRestored()
        {
            // Act
            var mapped = _mappingService.MapToStatisticsWrapper(StatisticsProductType.BuildingsRestored, new List<StatisticsProduct> { statisticsProduct })
                as ResponseWrapper<StatisticsBuildingsRestoredResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(statisticsProduct.TotalBuildingRestoredCount, model.TotalBuildingRestoredCount);
        }

        [Fact]
        public void MapIncidents()
        {
            // Act
            var mapped = _mappingService.MapToStatisticsWrapper(StatisticsProductType.Incidents, new List<StatisticsProduct> { statisticsProduct })
                as ResponseWrapper<StatisticsIncidentsResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(statisticsProduct.TotalIncidentCount, model.TotalIncidentCount);
        }

        [Fact]
        public void MapReports()
        {
            // Act
            var mapped = _mappingService.MapToStatisticsWrapper(StatisticsProductType.Reports, new List<StatisticsProduct> { statisticsProduct })
                as ResponseWrapper<StatisticsReportsResponseModel>;
            var model = mapped.Models.ToArray()[0];

            // Assert.
            Assert.Equal(statisticsProduct.TotalReportCount, model.TotalReportCount);
        }
    }
}
