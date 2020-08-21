using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Types;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice.Analysis
{
    /// <summary>
    ///     Integration test for the analysis controller.
    /// </summary>
    public class AnalysisTests : IClassFixture<WebserviceWebApplicationFactory>
    {
        private readonly WebserviceWebApplicationFactory _factory;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AnalysisTests(WebserviceWebApplicationFactory factory) => _factory = factory;

        [Theory]
        [InlineData(AnalysisProductTypeResponseModel.BuildingData)]
        [InlineData(AnalysisProductTypeResponseModel.BuildingDescription)]
        [InlineData(AnalysisProductTypeResponseModel.Complete)]
        [InlineData(AnalysisProductTypeResponseModel.Costs)]
        [InlineData(AnalysisProductTypeResponseModel.Foundation)]
        [InlineData(AnalysisProductTypeResponseModel.FoundationPlus)]
        [InlineData(AnalysisProductTypeResponseModel.Risk)]
        public async Task GetProductById(AnalysisProductTypeResponseModel product)
        {
            // Arrange.
            var expectedAnalysisProduct = new AnalysisProductFaker()
                .Generate();
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .RuleFor(f => f.NeighborhoodId, f => expectedAnalysisProduct.NeighborhoodId)
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedAnalysisProduct)
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={product}&id={expectedAnalysisProduct.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            Assert.Equal(expectedAnalysisProduct.Id, returnObject.Models.First().Id);
            // TODO Mapping test
        }

        [Theory]
        [InlineData(AnalysisProductTypeResponseModel.BuildingData)]
        [InlineData(AnalysisProductTypeResponseModel.BuildingDescription)]
        [InlineData(AnalysisProductTypeResponseModel.Complete)]
        [InlineData(AnalysisProductTypeResponseModel.Costs)]
        [InlineData(AnalysisProductTypeResponseModel.Foundation)]
        [InlineData(AnalysisProductTypeResponseModel.FoundationPlus)]
        [InlineData(AnalysisProductTypeResponseModel.Risk)]
        public async Task GetProductByExternalId(AnalysisProductTypeResponseModel product)
        {
            // Arrange.
            var expectedAnalysisProduct = new AnalysisProductFaker()
                .RuleFor(f => f.ExternalSource, f => ExternalDataSource.NlBag)
                .Generate();
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .RuleFor(f => f.NeighborhoodId, f => expectedAnalysisProduct.NeighborhoodId)
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedAnalysisProduct)
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={product}&bagid={expectedAnalysisProduct.ExternalId}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            Assert.Equal(expectedAnalysisProduct.Id, returnObject.Models.First().Id);
            // TODO Mapping test
        }

        [Theory]
        [InlineData(AnalysisProductTypeResponseModel.BuildingData)]
        [InlineData(AnalysisProductTypeResponseModel.BuildingDescription)]
        [InlineData(AnalysisProductTypeResponseModel.Complete)]
        [InlineData(AnalysisProductTypeResponseModel.Costs)]
        [InlineData(AnalysisProductTypeResponseModel.Foundation)]
        [InlineData(AnalysisProductTypeResponseModel.FoundationPlus)]
        [InlineData(AnalysisProductTypeResponseModel.Risk)]
        public async Task GetProductByQuery(AnalysisProductTypeResponseModel product)
        {
            // Arrange.
            var expectedAnalysisProduct = new AnalysisProductFaker()
                .Generate();
            var expectedStatisticsProduct = new StatisticsProductFaker()
                .RuleFor(f => f.NeighborhoodId, f => expectedAnalysisProduct.NeighborhoodId)
                .Generate();
            var client = _factory
                .WithObjectStoreItem(expectedAnalysisProduct)
                .WithObjectStoreItem(expectedStatisticsProduct)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={product}&query=thisismyquerystring&limit=1");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal((uint)1, returnObject.ModelCount);
            // TODO What else to test?
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
            // Arrange.
            var expectedAnalysisProducts = new AnalysisProductFaker().Generate(100);
            var expectedStatisticsProducts = new StatisticsProductFaker().Generate(100);
            // TODO Clean up
            for (int i = 0; i < 100; i ++)
            {
                expectedStatisticsProducts[i].NeighborhoodId = expectedAnalysisProducts[i].NeighborhoodId;
            }
            var client = _factory
                .WithObjectStoreList(expectedAnalysisProducts)
                .WithObjectStoreList(expectedStatisticsProducts)
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?limit={limit}&offset={offset}&product={AnalysisProductTypeResponseModel.Complete}&query=thisismyquerystring");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>();

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(limit, returnObject.ModelCount);
            // TODO What else to test?
        }

        [Fact]
        public async Task InvalidProductThrows()
        {
            // Arrange.
            var client = _factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product=135385");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TOOD Change when error handling is correct
        }

        [Theory]
        [InlineData("id=342ffdsfsd9478&bagid=4289374423")]
        [InlineData("id=342947dsf8&query=dskljhfkjshf")]
        [InlineData("bagid=sadfdsaf&query=myquery")]
        [InlineData("id=487239847&bagid=sadfdsaf&query=myquery")]
        public async Task MultipleRequestMethodsThrows(string queryString)
        {
            // Arrange.
            var client = _factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.BuildingData}&{queryString}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TOOD Change when error handling is correct
        }

        [Fact]
        public async Task NoRequestMethodThrows()
        {
            // Arrange.
            var client = _factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.BuildingData}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TODO Change when error handling is correct
        }

        [Theory]
        [InlineData("id=3kjhr834dhfjdeh")]
        [InlineData("bagid=4928374hfdkjsfh")]
        [InlineData("query=thisismyquerystringyes")]
        public async Task NoProductThrows(string queryString)
        {
            // Arrange.
            var client = _factory.CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?{queryString}");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TODO Change when error handling is correct
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 6)]
        public async Task InvalidNavigationThrows(uint limit, uint offset)

        {
            // Arrange.
            var client = _factory
                .CreateClient();

            // Act.
            var response = await client.GetAsync($"api/analysis/get?limit={limit}&offset={offset}&product={AnalysisProductTypeResponseModel.Complete}&query=thisismyquerystring");

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // TODO Change when error handling is correct
        }
    }
}
