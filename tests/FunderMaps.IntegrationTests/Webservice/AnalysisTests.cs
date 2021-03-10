using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types.Products;
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
    public class AnalysisTests : IClassFixture<WebserviceFixtureFactory>
    {
        private WebserviceFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance and setup the test data.
        /// </summary>
        public AnalysisTests(WebserviceFixtureFactory factory)
            => Factory = factory;

        [Theory]
        [InlineData(AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductType.Complete)]
        [InlineData(AnalysisProductType.RiskPlus)]
        public async Task GetProductByIdReturnProduct(AnalysisProductType product)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/product/analysis?product={product}&id=gfm-948cb2c2909c4c948de5ba468499b441");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.ItemCount);
            Assert.Equal("2622JN", returnObject.Items.First().PostalCode);
        }

        [Theory]
        [InlineData(AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductType.Complete)]
        [InlineData(AnalysisProductType.RiskPlus)]
        public async Task GetProductByExternalIdReturnProduct(AnalysisProductType product)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/product/analysis?product={product}&id=NL.IMBAG.PAND.0503100000000714");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.ItemCount);
            Assert.Equal("2622JM", returnObject.Items.First().PostalCode);
        }

        [Theory]
        [InlineData(AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductType.Complete)]
        [InlineData(AnalysisProductType.RiskPlus)]
        public async Task GetProductByExternalIdBag1ReturnProduct(AnalysisProductType product)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/product/analysis?product={product}&id=0503100000000714");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.ItemCount);
            Assert.Equal("2622JM", returnObject.Items.First().PostalCode);
        }

        [Theory]
        [InlineData(AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductType.Complete)]
        [InlineData(AnalysisProductType.RiskPlus)]
        public async Task GetProductByExternalAddressIdReturnProduct(AnalysisProductType product)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/product/analysis?product={product}&id=NL.IMBAG.NUMMERAANDUIDING.0503200000096292");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.ItemCount);
            Assert.Equal("2622JN", returnObject.Items.First().PostalCode);
        }

        [Theory]
        [InlineData(AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductType.Complete)]
        [InlineData(AnalysisProductType.RiskPlus)]
        public async Task GetProductByExternalAddressIdBag1ReturnProduct(AnalysisProductType product)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/product/analysis?product={product}&id=0503200000096292");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, returnObject.ItemCount);
            Assert.Equal("2622JN", returnObject.Items.First().PostalCode);
        }

        [Fact]
        public async Task GetByIdInvalidProductThrows()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/product/analysis?product=135385&id=gfm-948cb2c2909c4c948de5ba468499b441");

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
            var response = await client.GetAsync($"api/product/analysis?product={AnalysisProductType.Complete}&id={address}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetProductWithoutRequestMethodThrows()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/product/analysis?product={AnalysisProductType.Complete}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
