using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    // FUTURE: navigation test

    public class ContractorTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public ContractorTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllContractorReturnAllContractor()
        {
            // Arrange
            var organization = new OrganizationFaker().Generate(10);
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/contractor");
            var returnList = await response.Content.ReadFromJsonAsync<List<ContractorDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(10, returnList.Count);
            Assert.True(response.Headers.CacheControl.Public);
            Assert.NotNull(response.Headers.CacheControl.MaxAge);
        }
    }
}
