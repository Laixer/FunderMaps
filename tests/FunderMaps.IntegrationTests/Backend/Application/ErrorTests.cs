using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Application
{
    public class ErrorTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ErrorTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Theory]
        [InlineData("/")]
        [InlineData("/api")]
        [InlineData("/api/does-not-exist")]
        public async Task GetEndpointsReturnNotFound(string uri)
        {
            // Arrange
            using var client = Factory.CreateClient();

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
            using var client = Factory.CreateClient();

            // Act
            var response = await client.PostAsync(uri, null);

            // Assert
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}
