using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Faker;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public OrganizationTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetOrganizationFromSessionReturnSingleOrganization()
        {
            // Arrange
            var sessionUser = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                })
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/organization");
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sessionOrganization.Id, returnObject.Id);
        }

        [Fact]
        public async Task UpdateOrganizationFromSessionReturnNoContent()
        {
            // Arrange
            var newOrganization = new OrganizationFaker().Generate();
            var sessionUser = new UserFaker().Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                    options.OrganizationRole = OrganizationRole.Superuser;
                })
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync("api/organization", newOrganization);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task UpdateOrganizationFromSessionReturnForbidden(OrganizationRole organizationRole)
        {
            // Arrange
            var newOrganization = new OrganizationFaker().Generate();
            var client = _factory
                .ConfigureAuthentication(options =>
                {
                    options.OrganizationRole = organizationRole;
                })
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PutAsJsonAsync("api/organization", newOrganization);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
