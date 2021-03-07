using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Testing.Faker;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationAdminTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private AuthBackendWebApplicationFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationAdminTests(AuthBackendWebApplicationFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetOrganizationByIdReturnSingleOrganization()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();

            // Act
            var response = await client.GetAsync($"api/admin/organization/{Factory.Organization.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Factory.Organization.Id, returnObject.Id);
        }

        [Fact]
        public async Task GetAllOrganizationReturnPageOrganization()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            await Factory.CreateOrganizationAsync();

            // Act
            var response = await client.GetAsync("api/admin/organization");
            var returnList = await response.Content.ReadFromJsonAsync<List<OrganizationDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count >= 2);
        }

        [Fact]
        public async Task UpdateOrganizationReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();

            // Act
            var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}", new OrganizationFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrganizationReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync(track: false);

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetOrganizationByIdReturnForbidden()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/admin/organization/{Factory.Organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetAllOrganizationReturnForbidden()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/admin/organization");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdateOrganizationReturnForbidden()
        {
            // Arrange
            using var client = Factory.CreateClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();

            // Act
            var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}", new OrganizationFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrganizationReturnForbidden()
        {
            // Arrange
            using var client = Factory.CreateClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
