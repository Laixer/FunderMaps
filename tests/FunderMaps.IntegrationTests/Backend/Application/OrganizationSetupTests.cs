using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationSetupTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationSetupTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task SetupOrganizationReturnOrganization()
        {
            // Arrange
            using var adminClient = Factory.CreateAdminClient();

            // Act
            var response = await adminClient.PostAsJsonAsync("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationProposalDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Arrange
            using var client = Factory.CreateUnauthorizedClient();
            var newObject = new OrganizationSetupDtoFaker().Generate();

            // Act
            response = await client.PostAsJsonAsync($"api/organization/{returnObject.Id}/setup", newObject);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Act
            response = await client.PostAsJsonAsync("api/auth/signin", new SignInInputModel()
            {
                Email = newObject.Email,
                Password = newObject.Password,
            });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Act
            response = await adminClient.DeleteAsync($"api/admin/organization/{returnObject.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
