using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
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
            var organizationSetup = new OrganizationSetupDtoFaker().Generate();
            var client1 = new AuthBackendWebApplicationFactory()
                .ConfigureAuthentication(options => options.User.Role = ApplicationRole.Administrator)
                .WithAuthenticationStores()
                .CreateClient();
            var organization = await client1.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"api/organization/{organization.Id}/setup", organizationSetup);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
