using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Theory]
        [InlineData(OrganizationRole.Superuser)]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task GetOrganizationFromSessionReturnSingleOrganization(OrganizationRole role)
        {
            // Arrange
            using var client = Factory.CreateClient(role);

            // Act
            var response = await client.GetAsync("api/organization");
            var returnObject = await response.Content.ReadFromJsonAsync<OrganizationDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Guid.Parse("05203318-6c55-43c1-a6a6-bb8c83f930c3"), returnObject.Id);
            Assert.NotNull(returnObject.Name);
            Assert.NotNull(returnObject.Email);
        }

        [Fact]
        public async Task UpdateOrganizationFromSessionReturnNoContent()
        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Superuser);
            var updateObject = new OrganizationDtoFaker().Generate();

            // Act
            var response = await client.PutAsJsonAsync("api/organization", updateObject);

            // Act
            var returnObject = await client.GetFromJsonAsync<OrganizationDto>("api/organization");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            // Assert.Equal(updateObject.PhoneNumber, returnObject.PhoneNumber);
            // Assert.Equal(updateObject.BrandingLogo, returnObject.BrandingLogo);
        }

        [Theory]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task UpdateOrganizationFromSessionReturnForbidden(OrganizationRole role)
        {
            // Arrange
            using var client = Factory.CreateClient(role);

            // Act
            var response = await client.PutAsJsonAsync("api/organization", new OrganizationDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
