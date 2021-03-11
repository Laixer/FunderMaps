using System.Net;
using System.Threading.Tasks;
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
