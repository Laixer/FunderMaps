using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    // FUTURE: Test other roles
    public class ReviewerTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private AuthBackendWebApplicationFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ReviewerTests(AuthBackendWebApplicationFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetAllReviewerReturnAllReviewer()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/reviewer");
            var returnList = await response.Content.ReadFromJsonAsync<List<ReviewerDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count > 0);
        }
    }
}
