using FunderMaps.Core.Entities;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Application
{
    public class OrganizationTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        private readonly User sessionUser = new UserFaker().Generate();
        private readonly Organization sessionOrganization = new OrganizationFaker().Generate();

        public OrganizationTests(AuthWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory
                .WithAuthentication(options =>
                {
                    options.User = sessionUser;
                    options.Organization = sessionOrganization;
                    options.OrganizationRole = Core.Types.OrganizationRole.Reader;
                })
                .WithAuthenticationStores()
                .CreateClient();
        }

        [Fact]
        public async Task GetOrganizationFromSessionReturnSingleOrganization()
        {
            // Act
            var response = await _client.GetAsync("api/organization");
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

            // Act
            var response = await _client.PutAsJsonAsync("api/organization", newOrganization);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
