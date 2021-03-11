using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class ReviewerTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ReviewerTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Theory]
        [InlineData(OrganizationRole.Superuser)]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        public async Task GetAllReviewerReturnAllReviewer(OrganizationRole role)
        {
            // Arrange
            using var client = Factory.CreateClient(role);

            // Act
            var response = await client.GetAsync("api/reviewer");
            var returnList = await response.Content.ReadFromJsonAsync<List<ReviewerDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnList.Count > 0);
        }

        [Fact]
        public async Task GetAllReviewerReturnForbidden()
        {
            // Arrange
            using var client = Factory.CreateClient(OrganizationRole.Reader);

            // Act
            var response = await client.GetAsync("api/reviewer");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
