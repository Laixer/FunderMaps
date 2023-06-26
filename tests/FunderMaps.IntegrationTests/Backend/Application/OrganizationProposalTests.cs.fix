using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class OrganizationProposalTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationProposalTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task OrganizationProposalLifeCycle()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.GetAsync("api/organization/proposal/stats");
                var returnObject = await response.Content.ReadFromJsonAsync<DatasetStatsDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(returnObject.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.GetAsync($"api/organization/proposal/{organizationProposal.Id}");
                var returnObject = await response.Content.ReadFromJsonAsync<OrganizationProposalDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(organizationProposal.Id, returnObject.Id);
            }

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.GetAsync("api/organization/proposal");
                var returnList = await response.Content.ReadFromJsonAsync<List<OrganizationProposalDto>>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(returnList.Count >= 1);
            }

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.DeleteAsync($"api/organization/proposal/{organizationProposal.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact]
        public async Task OrganizationProposalLifeCycleForbidden()
        {
            var organizationProposal = await ApplicationStub.CreateProposalAsync(Factory);

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.PostAsJsonAsync("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync($"api/organization/proposal/{organizationProposal.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.GetAsync("api/organization/proposal");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateClient();

                // Act
                var response = await client.DeleteAsync($"api/organization/proposal/{organizationProposal.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }

            {
                // Arrange
                using var client = Factory.CreateAdminClient();

                // Act
                var response = await client.DeleteAsync($"api/organization/proposal/{organizationProposal.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
    }
}
