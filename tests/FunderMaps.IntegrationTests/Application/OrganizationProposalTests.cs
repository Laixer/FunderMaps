using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Application
{
    public class OrganizationProposalTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;

        public OrganizationProposalTests(AuthWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateOrganizationProposalReturnOrganizationProposal()
        {
            // Arrange
            var organization = new OrganizationProposalDtoFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/organization/proposal", organization);
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationProposalDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
        }

        [Fact]
        public async Task GetOrganizationProposaByIdReturnSingleOrganizationProposa()
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/organization/proposal/{organization.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationProposalDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
        }

        [Fact]
        public async Task GetAllOrganizationProposaReturnPageOrganizationProposa()
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate(0, 10);
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/organization/proposal");
            var returnList = await response.Content.ReadFromJsonAsync<List<OrganizationProposalDto>>().ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnList);
        }

        [Fact]
        public async Task DeleteOrganizationProposaReturnNoContent()
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate();
            var client = _factory
                .WithAuthentication()
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/organization/proposal/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
