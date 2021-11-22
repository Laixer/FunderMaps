using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Portal
{
    public class VersionsTests : IClassFixture<PortalWebApplicationFactory>
    {
        private PortalWebApplicationFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public VersionsTests(PortalWebApplicationFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetVersionUnauthorizedReturnSuccessAndCorrectContentType()
        {
            // Arrange
            using var client = Factory.CreateClient();

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
