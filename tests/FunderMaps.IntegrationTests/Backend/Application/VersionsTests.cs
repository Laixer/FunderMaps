using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class VersionsTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public VersionsTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetVersionUnauthorizedReturnSuccessAndCorrectContentType()
        {
            // Arrange
            using var client = Factory.CreateUnauthorizedClient();

            // Act
            var response = await client.GetAsync("api/version");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("json", response.Content.Headers.ContentType.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("utf-8", response.Content.Headers.ContentType.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.True(response.Headers.CacheControl.Public);
            Assert.NotNull(response.Headers.CacheControl.MaxAge);
        }

        [Fact]
        public async Task GetVersionAuthorizedReturnSuccessAndCorrectContentType()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/version");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
