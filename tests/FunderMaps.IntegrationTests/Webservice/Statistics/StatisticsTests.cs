using FunderMaps.IntegrationTests.Faker;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Statistics;
using FunderMaps.Webservice.ResponseModels.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice.Statistics
{
    /// TODO Test the mapping as well.
    /// <summary>
    ///     Integration test for the statistics controller.
    /// </summary>
    public class StatisticsTests : IClassFixture<WebserviceWebApplicationFactory>
    {
        private readonly WebserviceWebApplicationFactory _factory;

        // TODO QUESTION Is this the way to go?
        public static IEnumerable<object[]> AllProductTypes
        {
            get => Enum.GetValues(typeof(StatisticsProductTypeResponseModel))
                .Cast<StatisticsProductTypeResponseModel>()
                .Select(x => new object[] { x });
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StatisticsTests(WebserviceWebApplicationFactory factory) => _factory = factory;

        [Theory]
        [MemberData(nameof(AllProductTypes))]
        public async Task GetProductByNeighborhoodCode(StatisticsProductTypeResponseModel product)
        {
            // Arrange.
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={product}&neighborhoodCode={expectedStatisticsProduct.NeighborhoodCode}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsBuildingsRestoredResponseModel>>(); // TODO Can't grab the abstract base. This has no effect.

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            // TODO Mapping test
        }

        [Fact]
        public async Task InvalidProductThrows()
        {
            // Arrange.
            var client = _factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product=298376");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TOOD Change when error handling is correct
        }

        [Fact]
        public async Task NoRequestMethodThrows()
        {
            // Arrange.
            var client = _factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?product={StatisticsProductTypeResponseModel.FoundationRatio}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TODO Change when error handling is correct
        }

        [Theory]
        [InlineData("neigborhoodCode=3kjhr834dhfjdeh")]
        public async Task NoProductThrows(string queryString)
        {
            // Arrange.
            var client = _factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?{queryString}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TODO Change when error handling is correct
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 9)]
        public async Task InvalidNavigationThrows(uint limit, uint offset)

        {
            // Arrange.
            var client = _factory
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/statistics/get?limit={limit}&offset={offset}&product={StatisticsProductTypeResponseModel.ConstructionYears}&query=thisismyquerystring");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TODO Change when error handling is correct
        }
    }
}
