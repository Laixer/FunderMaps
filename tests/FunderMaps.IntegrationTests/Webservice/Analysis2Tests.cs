using FunderMaps.AspNetCore.DataTransferObjects;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice
{
    /// <summary>
    ///     Integration test for the analysis controller.
    /// </summary>
    public class Analysis2Tests : IClassFixture<WebserviceFixtureFactory>
    {
        private WebserviceFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance and setup the test data.
        /// </summary>
        public Analysis2Tests(WebserviceFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetProductByIdReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v2/product/analysis?id=gfm-ac31bec346744745b29f8505dff8182e");
            var returnObject = await response.Content.ReadFromJsonAsync<IList<AnalysisV2Dto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.Count);
            Assert.Equal("NL.IMBAG.LIGPLAATS.0503020000111954", returnObject.First().ExternalBuildingId);

            Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis2") > 0);
        }

        [Fact]
        public async Task GetProductByExternalIdReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v2/product/analysis?id=NL.IMBAG.LIGPLAATS.0503020000111954");
            var returnObject = await response.Content.ReadFromJsonAsync<IList<AnalysisV2Dto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.Count);
            Assert.Equal("gfm-ac31bec346744745b29f8505dff8182e", returnObject.First().BuildingId);

            Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis2") > 0);
        }

        [Fact]
        public async Task GetProductByExternalIdBag1ReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v2/product/analysis?id=0503020000111954");
            var returnObject = await response.Content.ReadFromJsonAsync<IList<AnalysisV2Dto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.Count);
            Assert.Equal("gfm-ac31bec346744745b29f8505dff8182e", returnObject.First().BuildingId);

            Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis2") > 0);
        }

        [Fact]
        public async Task GetProductByExternalAddressIdReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v2/product/analysis?id=NL.IMBAG.NUMMERAANDUIDING.0503200000111954");
            var returnObject = await response.Content.ReadFromJsonAsync<IList<AnalysisV2Dto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.Count);
            Assert.Equal("gfm-ac31bec346744745b29f8505dff8182e", returnObject.First().BuildingId);

            Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis2") > 0);
        }

        [Fact]
        public async Task GetProductByExternalAddressIdBag1ReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v2/product/analysis?id=0503200000111954");
            var returnObject = await response.Content.ReadFromJsonAsync<IList<AnalysisV2Dto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.Count);
            Assert.Equal("gfm-ac31bec346744745b29f8505dff8182e", returnObject.First().BuildingId);

            Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis2") > 0);
        }

        [Fact]
        public async Task GetRiskIndexByIdReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v2/product/at_risk?id=gfm-ac31bec346744745b29f8505dff8182e");
            var returnObject = await response.Content.ReadFromJsonAsync<bool>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(returnObject);

            Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis2") > 0);
        }

        [Fact]
        public async Task GetRiskIndexByExternalIdReturnProduct()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v2/product/at_risk?id=NL.IMBAG.LIGPLAATS.0503020000111954");
            var returnObject = await response.Content.ReadFromJsonAsync<bool>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(returnObject);

            Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis2") > 0);
        }

        [Theory]
        [InlineData("id=3kjhr834dhfjdeh")]
        [InlineData("bagid=4928374hfdkjsfh")]
        [InlineData("query=thisismyquerystringyes")]
        [InlineData("fdshjbf438gi")]
        public async Task GetRiskIndexByExternalIdInvalidAddressThrows(string address)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/v2/product/at_risk?id={address}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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

            // Act
            var response = await client.GetAsync($"api/v2/product/analysis?id={address}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
