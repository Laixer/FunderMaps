using FunderMaps.IntegrationTests.Faker;
using FunderMaps.IntegrationTests.Webservice.MappingValidation;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Statistics;
using FunderMaps.Webservice.ResponseModels.Types;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice.Statistics
{
    /// <summary>
    ///     Integration test for the statistics controller.
    /// </summary>
    public class StatisticsMappingTest : IClassFixture<WebserviceWebApplicationFactory>
    {
        private readonly WebserviceWebApplicationFactory _factory;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StatisticsMappingTest(WebserviceWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task MapFoundationRatio()
        {
            // Arrange.
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.FoundationRatio}&neighborhoodCode={expectedStatisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsFoundationRatioResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            MappingValidator.ValidateFoundationTypeDistribution(expectedStatisticsProduct.FoundationTypeDistribution, returnObject.Models.First().FoundationTypeDistribution);
        }


        [Fact]
        public async Task MapConstructionYears()
        {
            // Arrange.
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.ConstructionYears}&neighborhoodCode={expectedStatisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsConstructionYearsResonseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            MappingValidator.ValidateConstructionYearDistribution(expectedStatisticsProduct.ConstructionYearDistribution, returnObject.Models.First().ConstructionYearDistribution);
        }

        [Fact]
        public async Task MapFoundationRisk()
        {
            // Arrange.
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.FoundationRisk}&neighborhoodCode={expectedStatisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsFoundationRiskResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            MappingValidator.ValidateFoundationRiskDistribution(expectedStatisticsProduct.FoundationRiskDistribution, returnObject.Models.First().FoundationRiskDistribution);
        }

        [Fact]
        public async Task MapDataCollected()
        {
            // Arrange.
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.DataCollected}&neighborhoodCode={expectedStatisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsDataCollectedResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            Assert.Equal(expectedStatisticsProduct.DataCollectedPercentage, returnObject.Models.First().DataCollectedPercentage);
        }

        [Fact]
        public async Task MapBuildingsRestored()
        {
            // Arrange.
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.BuildingsRestored}&neighborhoodCode={expectedStatisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsBuildingsRestoredResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            Assert.Equal(expectedStatisticsProduct.TotalBuildingRestoredCount, returnObject.Models.First().TotalBuildingRestoredCount);
        }

        [Fact]
        public async Task MapIncidents()
        {
            // Arrange.
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.Incidents}&neighborhoodCode={expectedStatisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsIncidentsResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            Assert.Equal(expectedStatisticsProduct.TotalIncidentCount, returnObject.Models.First().TotalIncidentCount);
        }

        [Fact]
        public async Task MapReports()
        {
            // Arrange.
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.Reports}&neighborhoodCode={expectedStatisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsReportsResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            Assert.Equal(expectedStatisticsProduct.TotalReportCount, returnObject.Models.First().TotalReportCount);
        }
    }
}
