using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationProposalTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public OrganizationProposalTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateOrganizationProposalReturnOrganizationProposal()
        {
            // Arrange
            var organization = new OrganizationProposalDtoFaker().Generate();
            var client = _factory
                .WithAuthentication(options => options.User.Role = ApplicationRole.Administrator)
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
        public async Task GetOrganizationProposalByIdReturnSingleOrganizationProposa()
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate();
            var client = _factory
                .WithAuthentication(options => options.User.Role = ApplicationRole.Administrator)
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
        public async Task GetAllOrganizationProposalReturnPageOrganizationProposal()
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate(0, 10);
            var client = _factory
                .WithAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/organization/proposal");
            var returnList = await response.Content.ReadFromJsonAsync<List<OrganizationProposalDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnList);
        }

        [Fact]
        public async Task DeleteOrganizationProposalReturnNoContent()
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate();
            var client = _factory
                .WithAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/organization/proposal/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task CreateOrganizationProposalReturnForbidden(ApplicationRole role)
        {
            // Arrange
            var organization = new OrganizationProposalDtoFaker().Generate();
            var client = _factory
                .WithAuthentication(options => options.User.Role = role)
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/organization/proposal", organization);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task GetOrganizationProposalByIdReturnForbidden(ApplicationRole role)
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate();
            var client = _factory
                .WithAuthentication(options => options.User.Role = role)
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/organization/proposal/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task GetAllOrganizationProposalReturnForbidden(ApplicationRole role)
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate(0, 10);
            var client = _factory
                .WithAuthentication(options => options.User.Role = role)
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/organization/proposal");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task DeleteOrganizationProposalReturnForbidden(ApplicationRole role)
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate();
            var client = _factory
                .WithAuthentication(options => options.User.Role = role)
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/organization/proposal/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
