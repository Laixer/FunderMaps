using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class ContractorTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ContractorTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Theory]
        [InlineData(OrganizationRole.Superuser)]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task GetAllContractorReturnAllContractor(OrganizationRole role)
        {
            // Arrange
            using var client = Factory.CreateClient(role);

            // Act
            var response = await client.GetAsync("api/contractor");
            var returnList = await response.Content.ReadFromJsonAsync<List<ContractorDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count >= 1);
        }
    }
}
