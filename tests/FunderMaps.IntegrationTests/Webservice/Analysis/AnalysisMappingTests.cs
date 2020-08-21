using FunderMaps.Core.Interfaces;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.IntegrationTests.Webservice.MappingValidation;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Types;
using System;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice.Analysis
{
    /// TODO Lots of duplicate code.
    /// <summary>
    ///     Integration test for the analysis mapping functionality..
    /// </summary>
    public class AnalysisMappingTests : IClassFixture<WebserviceWebApplicationFactory>
    {
        private readonly WebserviceWebApplicationFactory _factory;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AnalysisMappingTests(WebserviceWebApplicationFactory factory) => _factory = factory ?? throw new ArgumentNullException(nameof(factory));

        [Fact]
        public async Task MapBuildingData()
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
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.BuildingData}&id={expectedAnalysisProduct.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisBuildingDataResponseModel>>();

            // Assert.
            MappingValidator.ValidateAnalysisBuildingData(expectedAnalysisProduct, returnObject.Models.First());
        }

        [Fact]
        public async Task MapFoundation()
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
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.Foundation}&id={expectedAnalysisProduct.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisFoundationResponseModel>>();

            // Assert.
            MappingValidator.ValidateAnalysisFoundation(expectedAnalysisProduct, returnObject.Models.First());
        }

        [Fact]
        public async Task MapFoundationPlus()
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
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.FoundationPlus}&id={expectedAnalysisProduct.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisFoundationPlusResponseModel>>();

            // Assert.
            MappingValidator.ValidateAnalysisFoundationPlus(expectedAnalysisProduct, returnObject.Models.First());
        }

        [Fact]
        public async Task MapCosts()
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
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.Costs}&id={expectedAnalysisProduct.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCostsResponseModel>>();

            // Assert.
            MappingValidator.ValidateAnalysisCosts(expectedAnalysisProduct, returnObject.Models.First());
        }

        [Fact]
        public async Task MapComplete()
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
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.Complete}&id={expectedAnalysisProduct.Id}");
            var returnObject = (await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisCompleteResponseModel>>()) as ResponseWrapper<AnalysisCompleteResponseModel>;

            // Assert.
            MappingValidator.ValidateAnalysisComplete(expectedAnalysisProduct, returnObject.Models.First());
        }

        // TODO Test IDescriptionService separately.
        [Fact]
        public async Task MapBuildingDescription()
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
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.BuildingDescription}&id={expectedAnalysisProduct.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisBuildingDescriptionResponseModel>>();

            // Assert.
            MappingValidator.ValidateAnalysisBuildingDescription(expectedAnalysisProduct, returnObject.Models.First());
        }

        [Fact]
        public async Task MapRisk()
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
            var response = await client.GetAsync($"api/analysis/get?product={AnalysisProductTypeResponseModel.Risk}&id={expectedAnalysisProduct.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<ResponseWrapper<AnalysisRiskResponseModel>>();

            // Assert.
            MappingValidator.ValidateAnalysisRisk(expectedAnalysisProduct, returnObject.Models.First());
        }

    }
}
