using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationUserTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task CreateOrganizationUserFromSessionReturnOrganizationUser()
        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);

            // Act
            var response = await client.PostAsJsonAsync("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());
            var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
        }

        [Fact]
        public async Task GetAllOrganizationUserFromSessionReturnAllOrganizationUser()
        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);

            // Act
            var response = await client.GetAsync("api/organization/user");
            var returnList = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count > 0); // TODO: update
        }

        [Fact]
        public async Task UpdateOrganizationUserFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.PutAsJsonAsync($"api/organization/user/{user.Id}", new UserDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ChangeOrganizationUserRoleFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.PostAsJsonAsync($"api/organization/user/{user.Id}/change-organization-role", new ChangeOrganizationRoleDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task ChangeOrganizationUserPasswordFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.PostAsJsonAsync($"api/organization/user/{user.Id}/change-password", new ChangePasswordDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrganizationUserFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);
            var user = await client.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Act
            var response = await client.DeleteAsync($"api/organization/user/{user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task CreateOrganizationUserFromSessionReturnForbidden(OrganizationRole role)
        {
            // Arrange
            using var client = Factory.CreateClient(role);

            // Act
            var response = await client.PostAsJsonAsync("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task UpdateOrganizationUserFromSessionReturnForbidden(OrganizationRole role)
        {
            // Arrange
            using var superuserClient = Factory.CreateClient(OrganizationRole.Superuser);
            var user = await superuserClient.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());
            using var client = Factory.CreateClient(role);

            // Act
            var response = await client.PutAsJsonAsync($"api/organization/user/{user.Id}", new UserDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task DeleteOrganizationUserFromSessionReturnForbidden(OrganizationRole role)
        {
            // Arrange
            using var superuserClient = Factory.CreateClient(OrganizationRole.Superuser);
            var user = await superuserClient.PostAsJsonGetFromJsonAsync<UserDto, OrganizationUserPasswordDto>("api/organization/user", new OrganizationUserPasswordDtoFaker().Generate());
            using var client = Factory.CreateClient(role);

            // Act
            var response = await client.DeleteAsync($"api/organization/user/{user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
