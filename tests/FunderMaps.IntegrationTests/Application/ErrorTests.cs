using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Application
{
    public class ErrorTests : IClassFixture<AuthWebApplicationFactory<Startup>>
    {
        private readonly AuthWebApplicationFactory<Startup> _factory;

        public ErrorTests(AuthWebApplicationFactory<Startup> factory)
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
                .WithAuthentication()
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
                .WithAuthentication()
                .WithAuthenticationStores()
                .CreateClient();

            // Act
            var response = await client.PostAsync(uri, null);

            // Assert
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}
