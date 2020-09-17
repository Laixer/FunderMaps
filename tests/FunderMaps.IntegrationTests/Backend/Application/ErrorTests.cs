using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class ErrorTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly AuthBackendWebApplicationFactory _factory;

        public ErrorTests(AuthBackendWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/api")]
        [InlineData("/api/does-not-exist")]
        public async Task GetEndpointsReturnNotFound(string uri)
        {
            // Arrange
            var client = _factory
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("api/version")]
        public async Task GetEndpointsReturnMethodNotAllowed(string uri)
        {
            // Arrange
            var client = _factory
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsync(uri, null);

            // Assert
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}
