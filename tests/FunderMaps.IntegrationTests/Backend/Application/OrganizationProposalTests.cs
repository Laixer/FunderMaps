using FunderMaps.Testing.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            // Arrange
            using var client = Factory.CreateAdminClient();
            var organizationProposal = await TestStub.CreateProposalAsync(Factory);

            {
                // Act
                var response = await client.GetAsync($"api/organization/proposal/{organizationProposal.Id}");
                var returnObject = await response.Content.ReadFromJsonAsync<OrganizationProposalDto>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            {
                // Act
                var response = await client.GetAsync("api/organization/proposal");
                var returnList = await response.Content.ReadFromJsonAsync<List<OrganizationProposalDto>>();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(returnList.Count >= 1);
            }

            {
                // Act
                var response = await client.DeleteAsync($"api/organization/proposal/{organizationProposal.Id}");

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact]
        public async Task CreateOrganizationProposalReturnForbidden()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetOrganizationProposalByIdReturnForbidden()
        {
            // Arrange
            using var adminClient = Factory.CreateAdminClient();
            var organization = await adminClient.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/organization/proposal/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetAllOrganizationProposalReturnForbidden()
        {
            // Arrange
            using var adminClient = Factory.CreateAdminClient();
            var organization = await adminClient.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/organization/proposal");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrganizationProposalReturnForbidden()
        {
            // Arrange
            using var adminClient = Factory.CreateAdminClient();
            var organization = await adminClient.PostAsJsonGetFromJsonAsync<OrganizationProposalDto, OrganizationProposalDto>("api/organization/proposal", new OrganizationProposalDtoFaker().Generate());
            using var client = Factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"api/organization/proposal/{organization.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
