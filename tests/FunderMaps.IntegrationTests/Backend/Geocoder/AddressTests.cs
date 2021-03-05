using FunderMaps.AspNetCore.DataTransferObjects;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Geocoder
{
    public class AddressTests : IClassFixture<AuthBackendWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AddressTests(AuthBackendWebApplicationFactory factory)
        {
            _client = factory
                .WithAuthenticationStores()
                .CreateClient();
        }

        [Theory]
        [InlineData("gfm-e64c5905ac7d412a8f075c1967da0271", "gfm-e64c5905ac7d412a8f075c1967da0271")]
        [InlineData("NL.IMBAG.NUMMERAANDUIDING.0503200000019584", "gfm-04fff5ffdfb543e1853cee4c2b64ddf6")]
        [InlineData("0503200000018972", "gfm-612986f77f12468d92ee545a0b8a5b84")]
        public async Task GetAddressByIdReturnSingleAddress(string address, string expected)
        {
            // Act
            var response = await _client.GetAsync($"api/address/{address}");
            var returnObject = await response.Content.ReadFromJsonAsync<AddressBuildingDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected, returnObject.AddressId);
        }
    }
}
