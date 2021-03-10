using FunderMaps.AspNetCore.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice
{
    /// <summary>
    ///     Integration test for the statistics controller.
    /// </summary>
    public class StatisticsTests : IClassFixture<WebserviceFixtureFactory>
    {
        private WebserviceFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance and setup the test data.
        /// </summary>
        public StatisticsTests(WebserviceFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetProductByIdReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/product/statistics?id=gfm-6aae47cb5aa4416abdf19d98ba8218ac");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsDto>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnObject.ItemCount >= 1);
        }

        [Fact]
        public async Task GetProductByExternalIdReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/product/statistics?id=BU05031403");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<StatisticsDto>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnObject.ItemCount >= 1);
        }

        [Theory]
        [InlineData("id=3kjhr834dhfjdeh")]
        [InlineData("bagid=4928374hfdkjsfh")]
        [InlineData("query=thisismyquerystringyes")]
        [InlineData("fdshjbf438gi")]
        public async Task GetByIdInvalidAddressThrows(string address)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/product/statistics?id={address}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
