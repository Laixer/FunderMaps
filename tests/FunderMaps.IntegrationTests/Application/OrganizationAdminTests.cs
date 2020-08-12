using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Application
{
    public class OrganizationAdminTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;

        public OrganizationAdminTests(AuthWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetOrganizationByIdReturnSingleOrganization()
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .WithAuthentication(options =>
                {
                    options.User.Role = ApplicationRole.Administrator;
                })
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/organization/{organization.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(organization.Id, returnObject.Id);
        }

        [Fact]
        public async Task UpdateOrganizationReturnNoContent()
        {
            // Arrange
            var newOrganization = new OrganizationFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .WithAuthentication(options =>
                {
                    options.User.Role = ApplicationRole.Administrator;
                })
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/organization/{organization.Id}", newOrganization);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task UpdateOrganizationReturnForbidden(ApplicationRole applicationRole)
        {
            // Arrange
            var newOrganization = new OrganizationFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .WithAuthentication(options =>
                {
                    options.User.Role = applicationRole;
                })
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/organization/{organization.Id}", newOrganization);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
