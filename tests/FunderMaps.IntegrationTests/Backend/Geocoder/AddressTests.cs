using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Geocoder
{
    public class AddressTests : IClassFixture<BackendFixtureFactory>
    {
        private BackendFixtureFactory Factory { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AddressTests(BackendFixtureFactory factory)
            => Factory = factory;

        [Fact]
        public async Task GetAddressByIdReturnSingleAddress()
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/address/gfm-35952a0487304368b0069b4485a69b4b");
            var returnObject = await response.Content.ReadFromJsonAsync<AddressDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("gfm-35952a0487304368b0069b4485a69b4b", returnObject.Id);
            Assert.Equal("gfm-0b50e18111754ed3b5c1408f9f64bf22", returnObject.BuildingId);
            Assert.Equal("4d", returnObject.BuildingNumber);
            Assert.Equal("2612PA", returnObject.PostalCode);
            Assert.Equal("Poortweg", returnObject.Street);
            Assert.Equal("Delft", returnObject.City);
        }

        [Theory]
        [InlineData("gfm-6d70df27db5347f88d932faa3a72d3b3", "gfm-6d70df27db5347f88d932faa3a72d3b3")]
        [InlineData("NL.IMBAG.NUMMERAANDUIDING.0503200000018943", "gfm-09e3b90972de425ea140ae27e49d60b5")]
        [InlineData("0503200000019289", "gfm-9ecb0f685cb84355ae464e2a358ac158")]
        public async Task GetAddressByGeoIdReturnSingleAddress(string address, string expected)
        {
            // Arrange
            using var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/address/{address}");
            var returnObject = await response.Content.ReadFromJsonAsync<AddressDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected, returnObject.Id);
        }

        [Theory]
        [InlineData(OrganizationRole.Superuser)]
        [InlineData(OrganizationRole.Verifier)]
        [InlineData(OrganizationRole.Writer)]
        [InlineData(OrganizationRole.Reader)]
        public async Task GetAddressByIdReturnOk(OrganizationRole role)
        {
            // Arrange
            using var client = Factory.CreateClient(role);

            // Act
            var response = await client.GetAsync($"api/address/gfm-6d70df27db5347f88d932faa3a72d3b3");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
