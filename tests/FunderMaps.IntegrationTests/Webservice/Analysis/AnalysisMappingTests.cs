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
            MappingValidator.Validate(expectedAnalysisProduct, returnObject.Models.First());
        }
    }
}
