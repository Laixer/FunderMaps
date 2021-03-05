using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.AspNetCore.DataTransferObjects;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Geocoder
{
    // FUTURE: Use DTO
    public class AddressTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AddressTests(AuthBackendWebApplicationFactory factory)
        {
            _client = factory
                .WithAuthenticationStores()
                .CreateClient();
        }

        [Fact]
        public async Task GetAddressByIdReturnSingleAddress()
        {
            // Act
            var response = await _client.GetAsync("api/address/gfm-e64c5905ac7d412a8f075c1967da0271");
            var returnObject = await response.Content.ReadFromJsonAsync<AddressBuildingDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("gfm-e64c5905ac7d412a8f075c1967da0271", returnObject.AddressId);
        }

        // [Theory]
        // [InlineData("kade")]
        // [InlineData("4621E")]
        // [InlineData("5707KM")]
        // [InlineData("straat")]
        // [InlineData("hage")]
        // public async Task GetAllAddressByQueryReturnMatchingAddressList(string query)
        // {
        //     // Act
        //     var response = await _client.GetAsync($"api/address/suggest?query={query}");
        //     var returnList = await response.Content.ReadFromJsonAsync<List<AddressBuildingDto>>();

        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.True(returnList.Count > 0);
        // }
    }
}
