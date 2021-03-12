using FunderMaps.WebApi.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Geocoder
{
    public class LayerTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public LayerTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetLayerByIdReturnOk()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/layer/73258fb5-54a4-4d0c-a85f-e0ca2313e67f");
            var returnObject = await response.Content.ReadFromJsonAsync<LayerDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Funderingsrisico", returnObject.Name);
        }

        [Fact]
        public async Task GetAllLayerReturnGetAllLayer()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/layer");
            var returnObject = await response.Content.ReadFromJsonAsync<List<LayerDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnObject.Count >= 10);
        }
    }
}
