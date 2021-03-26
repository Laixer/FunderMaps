using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Maplayer
{
    public class BundleTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetAllBundleReturnGetAllBundle()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/bundle");
            var returnObject = await response.Content.ReadFromJsonAsync<List<BundleDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnObject.Count >= 1);
            Assert.True(response.Headers.CacheControl.Private);
        }
    }
}
