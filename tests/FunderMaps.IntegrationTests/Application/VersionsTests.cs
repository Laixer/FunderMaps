using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Application
{
    public class VersionsTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public VersionsTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetEndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/version").ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("json", response.Content.Headers.ContentType.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains("utf-8", response.Content.Headers.ContentType.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.True(response.Headers.CacheControl.Public);
            Assert.NotNull(response.Headers.CacheControl.MaxAge);
        }
    }
}
