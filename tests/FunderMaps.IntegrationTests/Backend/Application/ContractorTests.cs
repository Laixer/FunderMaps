using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    // FUTURE: Test other roles
    public class ContractorTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private AuthBackendWebApplicationFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ContractorTests(AuthBackendWebApplicationFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetAllContractorReturnAllContractor()
        {
            // Arrange
            using var client = Factory.CreateClient();
            await Factory.CreateOrganizationAsync();

            // Act
            var response = await client.GetAsync("api/contractor");
            var returnList = await response.Content.ReadFromJsonAsync<List<ContractorDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count >= 2);
        }
    }
}
