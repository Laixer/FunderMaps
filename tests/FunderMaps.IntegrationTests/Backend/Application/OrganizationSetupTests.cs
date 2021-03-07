using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationSetupTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private AuthBackendWebApplicationFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationSetupTests(AuthBackendWebApplicationFactory factory)
            => Factory = factory;

        [Fact]
        public async Task SetupOrganizationReturnOrganization()
        {
            // Arrange
            using var adminClient = Factory.CreateAdminClient();
            var organization = await adminClient.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.PostAsJsonAsync($"api/organization/{organization.Id}/setup", new OrganizationSetupDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
