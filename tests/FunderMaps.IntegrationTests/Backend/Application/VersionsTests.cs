using System.Net;
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
            await TestStub.VersionAsync(Factory);
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
