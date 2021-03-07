using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Testing.Faker;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationUserAdminTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private AuthBackendWebApplicationFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserAdminTests(AuthBackendWebApplicationFactory factory)
            => Factory = factory;

        [Fact]
        public async Task CreateOrganizationUserReturnOrganizationUser()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());
            var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
        }

        [Fact]
        public async Task GetAllOrganizationUserReturnAllOrganizationUser()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();
            await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.GetAsync($"api/admin/organization/{organization.Id}/user");
            var returnList = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count >= 2);
        }

        [Fact]
        public async Task UpdateOrganizationUserReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}/user/{user.Id}", new UserDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ChangeOrganizationUserRoleReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user/{user.Id}/change-organization-role", new ChangeOrganizationRoleDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ChangeOrganizationUserPasswordReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user/{user.Id}/change-password", new ChangePasswordDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrganizationUserReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}/user/{user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrganizationUserReturnForbidden()
        {
            // Arrange
            using var client = Factory.CreateClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetAllOrganizationUserReturnForbidden()
        {
            // Arrange
            using var adminClient = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();
            await adminClient.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/admin/organization/{organization.Id}/user");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdateOrganizationUserReturnForbidden()
        {
            // Arrange
            using var adminClient = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();
            var user = await adminClient.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}/user/{user.Id}", new UserDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrganizationUserReturnForbidden()
        {
            // Arrange
            using var adminClient = Factory.CreateAdminClient();
            var (_, organization, _) = await Factory.CreateOrganizationAsync();
            var user = await adminClient.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>($"api/admin/organization/{organization.Id}/user", new OrganizationUserPasswordDtoFaker().Generate());
            using var client = Factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}/user/{user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
