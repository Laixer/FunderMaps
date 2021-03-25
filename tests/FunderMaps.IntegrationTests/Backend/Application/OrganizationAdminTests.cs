using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.IntegrationTests.Faker;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationAdminTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationAdminTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task OrganizationLifeCycle()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);
            var organizationSetup = await ApplicationStub.CreateOrganizationAsync(Factory, organizationProposal);
            var organization = await ApplicationStub.GetOrganizationAsync(Factory, organizationProposal);

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.GetAsync("api/admin/organization");
                var returnList = await response.Content.ReadFromJsonAsync<List<OrganizationDto>>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(returnList.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}", new OrganizationDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            await ApplicationStub.DeleteOrganizationAsync(Factory, organization);
        }

        [Fact]
        public async Task OrganizationLifeCycleForbidden()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);
            var organizationSetup = await ApplicationStub.CreateOrganizationAsync(Factory, organizationProposal);
            var organization = await ApplicationStub.GetOrganizationAsync(Factory, organizationProposal);

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/admin/organization/{organization.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync("api/admin/organization");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.PutAsJsonAsync($"api/admin/organization/{organization.Id}", new OrganizationDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.DeleteAsync($"api/admin/organization/{organization.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            await ApplicationStub.DeleteOrganizationAsync(Factory, organization);
        }
    }
}
