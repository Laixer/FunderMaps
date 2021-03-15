using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using Xunit;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     Generate random theory data.
    /// </summary>
    public static class TestStub
    {
        public static async Task VersionAsync<TStartup>(FixtureFactory<TStartup> factory)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateUnauthorizedClient();

            // Act
            var response = await client.GetAsync("api/version");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("json", response.Content.Headers.ContentType.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("utf-8", response.Content.Headers.ContentType.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.True(response.Headers.CacheControl.Public);
            Assert.NotNull(response.Headers.CacheControl.MaxAge);
        }

        public static async Task<SignInSecurityTokenDto> LoginAsync<TStartup>(FixtureFactory<TStartup> factory, string username, string password)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = username,
                Password = password,
            });
            var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject.Id);
            Assert.NotNull(returnObject.Token);
            Assert.NotNull(returnObject.Issuer);
            Assert.True(returnObject.ValidTo > returnObject.ValidFrom);

            return returnObject;
        }

        public static async Task<OrganizationProposalDto> CreateProposalAsync<TStartup>(FixtureFactory<TStartup> factory)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateAdminClient();
            var newObject = new OrganizationProposalDtoFaker().Generate();

            // Act
            var response = await client.PostAsJsonAsync("api/organization/proposal", newObject);
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationProposalDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(newObject.Name, returnObject.Name);

            return returnObject;
        }

        public static async Task<OrganizationSetupDto> CreateOrganizationAsync<TStartup>(FixtureFactory<TStartup> factory, OrganizationProposalDto organization)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateUnauthorizedClient();
            var newObject = new OrganizationSetupDtoFaker().Generate();

            // Act
            var response = await client.PostAsJsonAsync($"api/organization/{organization.Id}/setup", newObject);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            return newObject;
        }

        public static async Task<OrganizationDto> GetOrganizationAsync<TStartup>(FixtureFactory<TStartup> factory, OrganizationProposalDto organization)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateAdminClient();

            // Act
            var response = await client.GetAsync($"api/admin/organization/{organization.Id}");
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(organization.Id, returnObject.Id);

            return returnObject;
        }

        public static async Task RemoveOrganizationAsync<TStartup>(FixtureFactory<TStartup> factory, OrganizationDto organization)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateAdminClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        public static async Task<OrganizationUserPasswordDto> CreateOrganizationUserAsync<TStartup>(FixtureFactory<TStartup> factory, OrganizationDto organization)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateAdminClient();
            var newObject = new OrganizationUserPasswordDtoFaker().Generate();

            // Act
            var response = await client.PostAsJsonAsync($"api/admin/organization/{organization.Id}/user", newObject);
            var returnObject = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(newObject.GivenName, returnObject.GivenName);
            Assert.Equal(newObject.LastName, returnObject.LastName);
            Assert.Equal(newObject.Avatar, returnObject.Avatar);

            // FUTURE: Add more checks.

            return newObject with
            {
                Id = returnObject.Id
            };
        }

        public static async Task DeleteOrganizationUserAsync<TStartup>(FixtureFactory<TStartup> factory, OrganizationDto organization, UserDto user)
            where TStartup : class
        {
            // Arrange
            using var client = factory.CreateAdminClient();

            // Act
            var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}/user/{user.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
