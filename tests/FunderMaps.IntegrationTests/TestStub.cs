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
    }
}
