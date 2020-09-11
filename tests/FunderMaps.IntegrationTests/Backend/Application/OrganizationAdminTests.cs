using FunderMaps.Core.Types;
using FunderMaps.Testing.Extensions;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationAdminTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public OrganizationAdminTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetOrganizationByIdReturnSingleOrganization()
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/admin/organization/{organization.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(organization.Id, returnObject.Id);
        }

        [Fact]
        public async Task GetAllOrganizationReturnPageOrganization()
        {
            // Arrange
            var organization = new OrganizationFaker().Generate(0, 10);
            var client = _factory
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/admin/organization");
            var returnList = await response.Content.ReadFromJsonAsync<List<OrganizationDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnList);
        }

        [Fact]
        public async Task UpdateOrganizationReturnNoContent()
        {
            // Arrange
            var newOrganization = new OrganizationFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}", newOrganization);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrganizationReturnNoContent()
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task GetOrganizationByIdReturnForbidden(ApplicationRole role)
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options => options.User.Role = role)
                .WithAuthenticationStores()
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/admin/organization/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task GetAllOrganizationReturnForbidden(ApplicationRole role)
        {
            // Arrange
            var organization = new OrganizationFaker().Generate(0, 10);
            var client = _factory
                .ConfigureAuthentication(options => options.User.Role = role)
                .WithAuthenticationStores()
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/admin/organization");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task UpdateOrganizationReturnForbidden(ApplicationRole role)
        {
            // Arrange
            var newOrganization = new OrganizationFaker().Generate();
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options => options.User.Role = role)
                .WithAuthenticationStores()
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}", newOrganization);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(ApplicationRole.User)]
        [InlineData(ApplicationRole.Guest)]
        public async Task DeleteOrganizationReturnForbidden(ApplicationRole role)
        {
            // Arrange
            var organization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options => options.User.Role = role)
                .WithAuthenticationStores()
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
