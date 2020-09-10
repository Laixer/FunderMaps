using FunderMaps.Core.Types.Products;
using FunderMaps.Testing.Faker;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Statistics;
using FunderMaps.Webservice.ResponseModels.Types;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice
{
    /// <summary>
    ///     Integration test for the statistics controller.
    /// </summary>
    public class StatisticsTests : IClassFixture<AuthWebserviceWebApplicationFactory>
    {
        private readonly HttpClient client;
        private readonly StatisticsProduct statisticsProduct;

        /// <summary>
        ///     Create new instance and setup the test data.
        /// </summary>
        public StatisticsTests(AuthWebserviceWebApplicationFactory factory)
        {
            // Arrange.
            statisticsProduct = new StatisticsProductFaker().Generate();
            client = factory
                .WithAuthenticationStores()
                .WithDataStoreList(statisticsProduct)
                .CreateClient();
        }

        [Theory]
        [InlineData(StatisticsProductTypeResponseModel.BuildingsRestored)]
        [InlineData(StatisticsProductTypeResponseModel.ConstructionYears)]
        [InlineData(StatisticsProductTypeResponseModel.DataCollected)]
        [InlineData(StatisticsProductTypeResponseModel.FoundationRatio)]
        [InlineData(StatisticsProductTypeResponseModel.FoundationRisk)]
        [InlineData(StatisticsProductTypeResponseModel.Incidents)]
        [InlineData(StatisticsProductTypeResponseModel.Reports)]
        public async Task GetProductByNeighborhoodCodeReturnProduct(StatisticsProductTypeResponseModel product)
        {
            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={product}&neighborhoodCode={statisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsBuildingsRestoredResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
        }

        [Fact]
        public async Task GetInvalidProductThrows()
        {
            // Act.
            var response = await client.GetAsync($"api/statistics/get?product=298376");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }

        [Fact]
        public async Task GetProductWithoutRequestMethodThrows()
        {
            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.FoundationRatio}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }

        [Theory]
        [InlineData("neigborhoodCode=3kjhr834dhfjdeh")]
        public async Task GetByQueryWithoutProductThrows(string queryString)
        {
            // Act.
            var response = await client.GetAsync($"api/statistics/get?{queryString}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 9)]
        public async Task GetProductByQueryInvalidNavigationThrows(uint limit, uint offset)

        {
            // Act.
            var response = await client.GetAsync($"api/statistics/get?limit={limit}&offset={offset}&product={StatisticsProductTypeResponseModel.ConstructionYears}&query=thisismyquerystring");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }
    }
}
