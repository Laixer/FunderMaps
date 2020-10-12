using FunderMaps.Testing.Faker;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationSetupTests : IClassFixture<BackendWebApplicationFactory>
    {
        private readonly BackendWebApplicationFactory _factory;

        public OrganizationSetupTests(BackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SetupOrganizationReturnOrganization()
        {
            // Arrange
            var organization = new OrganizationProposalFaker().Generate();
            var organizationSetup = new OrganizationSetupDtoFaker().Generate();
            var client = _factory
                .WithDataStoreItem(organization)
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"api/organization/{organization.Id}/setup", organizationSetup);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
