using FunderMaps.Core.Entities;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests
{
    public class AddressTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public AddressTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAddressByIdReturnSingleAddress()
        {
            // Arrange
            var expectedAddress = new Address
            {
                Id = "gfm-000000e718f94fd8a8d502885d0d50ce",
                BuildingNumber = "117",
                PostalCode = "3245TM",
                Street = "Reiger",
                IsActive = true,
                ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0559200000005982",
                ExternalSource = "bag",
            };
            var client = _factory
                .WithDataStoreList(new[] { expectedAddress })
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/address/gfm-000000e718f94fd8a8d502885d0d50ce").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualAddress = await response.Content.ReadFromJsonAsync<Address>().ConfigureAwait(false);

            // Assert
            Assert.Equal(expectedAddress.Id, actualAddress.Id);
            Assert.Equal(expectedAddress.BuildingNumber, actualAddress.BuildingNumber);
            Assert.Equal(expectedAddress.PostalCode, actualAddress.PostalCode);
            Assert.Equal(expectedAddress.Street, actualAddress.Street);
            Assert.Equal(expectedAddress.IsActive, actualAddress.IsActive);
            Assert.Equal(expectedAddress.ExternalId, actualAddress.ExternalId);
            Assert.Equal(expectedAddress.ExternalSource, actualAddress.ExternalSource);
        }
    }
}
