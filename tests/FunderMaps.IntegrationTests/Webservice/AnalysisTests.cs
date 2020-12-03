#if _DISABLE
using FunderMaps.Core.Types.Products;
using FunderMaps.Testing.Faker;
using FunderMaps.Webservice.ResponseModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice
{
    /// <summary>
    ///     Integration test for the analysis controller.
    /// </summary>
    public class AnalysisTests : IClassFixture<AuthWebserviceWebApplicationFactory>
    {
        private const int ItemCount = 100;

        private readonly HttpClient client;
        private readonly List<AnalysisProduct> analysisProducts;
        private readonly List<StatisticsProduct> statisticsProducts;

        /// <summary>
        ///     Create new instance and setup the test data.
        /// </summary>
        public AnalysisTests(AuthWebserviceWebApplicationFactory factory)
        {
            // Arrange.
            analysisProducts = new AnalysisProductFaker().Generate(ItemCount);
            statisticsProducts = new StatisticsProductFaker().Generate(ItemCount);

            // Link analysis product to statistics product by neighborhood id.
            for (int i = 0; i < ItemCount; i++)
            {
                statisticsProducts[i].NeighborhoodId = analysisProducts[i].NeighborhoodId;
            }

            client = factory
                .WithAuthenticationStores()
                .WithDataStoreList(analysisProducts)
                .WithDataStoreList(statisticsProducts)
                .CreateClient();
        }

        [Theory]
        [InlineData(AnalysisProductType.BuildingData)]
        [InlineData(AnalysisProductType.Complete)]
        [InlineData(AnalysisProductType.Costs)]
        [InlineData(AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductType.FoundationPlus)]
        [InlineData(AnalysisProductType.Risk)]
        public async Task GetProductByIdReturnProduct(AnalysisProductType product)
        {
            // Arrange.
            var expectedAnalysisProduct = analysisProducts[0];

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={product}&id={expectedAnalysisProduct.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1U, returnObject.ModelCount);
            Assert.Equal(expectedAnalysisProduct.Id, returnObject.Models.First().Id);
        }

        [Theory]
        [InlineData(AnalysisProductType.BuildingData)]
        [InlineData(AnalysisProductType.Complete)]
        [InlineData(AnalysisProductType.Costs)]
        [InlineData(AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductType.FoundationPlus)]
        [InlineData(AnalysisProductType.Risk)]
        public async Task GetProductByExternalIdReturnProduct(AnalysisProductType product)
        {
            // Arrange.
            var expectedAnalysisProduct = analysisProducts[0];

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={product}&bagid={expectedAnalysisProduct.ExternalId}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1U, returnObject.ModelCount);
            Assert.Equal(expectedAnalysisProduct.Id, returnObject.Models.First().Id);
        }

        [Theory]
        [InlineData(AnalysisProductType.BuildingData)]
        [InlineData(AnalysisProductType.Complete)]
        [InlineData(AnalysisProductType.Costs)]
        [InlineData(AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductType.FoundationPlus)]
        [InlineData(AnalysisProductType.Risk)]
        public async Task GetProductByQueryReturnProduct(AnalysisProductType product)
        {
            // Arrange.
            var expectedAnalysisProduct = analysisProducts[0];
            var expectedStatisticsProduct = statisticsProducts[1];

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={product}&query=thisismyquerystring&limit=1");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1U, returnObject.ModelCount);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(16, 0)]
        [InlineData(55, 0)]
        [InlineData(1, 3)]
        [InlineData(16, 7)]
        [InlineData(55, 4)]
        public async Task Navigation(uint limit, uint offset)
        {
            // Act.
            var response = await client.GetAsync($"api/analysis/get?limit={limit}&offset={offset}&product={AnalysisProductType.Complete}&query=thisismyquerystring");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(limit, returnObject.ModelCount);
        }

        [Fact]
        public async Task GetByIdInvalidProductThrows()
        {
            // Act.
            var response = await client.GetAsync($"api/analysis/get?product=135385&id=fsdhfkdljshfkljh");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }

        [Theory]
        [InlineData(AnalysisProductType.BuildingData, "id=342ffdsfsd9478&bagid=4289374423")]
        [InlineData(AnalysisProductType.BuildingData, "id=342947dsf8&query=dskljhfkjshf")]
        [InlineData(AnalysisProductType.BuildingData, "bagid=sadfdsaf&query=myquery")]
        [InlineData(AnalysisProductType.BuildingData, "id=487239847&bagid=sadfdsaf&query=myquery")]
        public async Task GetProductByMultipleRequestMethodsThrows(AnalysisProductType product, string queryString)
        {
            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={product}&{queryString}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }

        [Fact]
        public async Task GetProductWithoutRequestMethodThrows()
        {
            // Act.
            var product = AnalysisProductType.BuildingData;
            var response = await client.GetAsync($"api/analysis/get?product={product}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }

        [Theory]
        [InlineData("id=3kjhr834dhfjdeh")]
        [InlineData("bagid=4928374hfdkjsfh")]
        [InlineData("query=thisismyquerystringyes")]
        public async Task GetWithoutProductThrows(string queryString)
        {
            // Act.
            var response = await client.GetAsync($"api/analysis/get?{queryString}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 6)]
        public async Task GetProductInvalidNavigationThrows(uint limit, uint offset)
        {
            // Act
            var response = await client.GetAsync($"api/analysis/get?limit={limit}&offset={offset}&product={AnalysisProductType.Complete}&query=thisismyquerystring");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // FUTURE Change when error handling is correct
        }
    }
}
#endif