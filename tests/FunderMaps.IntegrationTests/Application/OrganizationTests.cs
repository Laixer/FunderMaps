using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Application
{
    public class OrganizationTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;

        public OrganizationTests(AuthWebApplicationFactory<Startup> factory)
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
                .WithAuthentication(options =>
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
                .WithAuthentication(options =>
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
        [InlineData(ApplicationRole.Administrator, OrganizationRole.Verifier)]
        [InlineData(ApplicationRole.Administrator, OrganizationRole.Writer)]
        [InlineData(ApplicationRole.Administrator, OrganizationRole.Reader)]
        [InlineData(ApplicationRole.User, OrganizationRole.Verifier)]
        [InlineData(ApplicationRole.User, OrganizationRole.Writer)]
        [InlineData(ApplicationRole.User, OrganizationRole.Reader)]
        [InlineData(ApplicationRole.Guest, OrganizationRole.Verifier)]
        [InlineData(ApplicationRole.Guest, OrganizationRole.Writer)]
        [InlineData(ApplicationRole.Guest, OrganizationRole.Reader)]
        public async Task UpdateOrganizationFromSessionReturnForbidden(ApplicationRole applicationRole, OrganizationRole organizationRole)
        {
            // Arrange
            var newOrganization = new OrganizationFaker().Generate();
            var sessionUser = new UserFaker().RuleFor(f => f.Role, f => applicationRole).Generate();
            var sessionOrganization = new OrganizationFaker().Generate();
            var client = _factory
                .WithAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
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
