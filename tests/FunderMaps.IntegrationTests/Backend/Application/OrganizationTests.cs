using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Testing.Faker;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private AuthBackendWebApplicationFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationTests(AuthBackendWebApplicationFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetOrganizationFromSessionReturnSingleOrganization()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/organization");
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Factory.Organization.Id, returnObject.Id);
        }

        [Fact]
        public async Task UpdateOrganizationFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PutAsJsonAsync("api/organization", new OrganizationFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        // TODO:
        // [Theory]
        // [InlineData(OrganizationRole.Verifier)]
        // [InlineData(OrganizationRole.Writer)]
        // [InlineData(OrganizationRole.Reader)]
        // public async Task UpdateOrganizationFromSessionReturnForbidden(OrganizationRole organizationRole)
        // {
        //     // Arrange
        //     using var client = Factory.CreateClient();

        //     // Act
        //     var response = await client.PutAsJsonAsync("api/organization", new OrganizationFaker().Generate());

        //     // Assert
        //     Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        // }
    }
}
