using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Application
{
    public class OrganizationSetupTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;

        public OrganizationSetupTests(AuthWebApplicationFactory<Startup> factory)
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
                .WithDataStoreList(organization)
                .CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"api/organization/{organization.Id}/setup", organizationSetup);
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(organization.Id, returnObject.Id);
        }
    }
}
