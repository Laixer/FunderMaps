using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Backend.Geocoder;

// TODO: Include test for inactive address
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
        var response = await client.GetAsync($"api/address/gfm-059f268aad9d43339afe56a32cf641cc");
        var returnObject = await response.Content.ReadFromJsonAsync<Address>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-059f268aad9d43339afe56a32cf641cc", returnObject.Id);
        Assert.Equal("gfm-629e12e409bb4d0893640ae9ef3a53b7", returnObject.BuildingId);
        Assert.Equal("319b", returnObject.BuildingNumber);
        Assert.Equal("3023DG", returnObject.PostalCode);
        Assert.Equal("Rochussenstraat", returnObject.Street);
        Assert.Equal("Rotterdam", returnObject.City);
        Assert.True(response.Headers.CacheControl?.Public);
    }

    [Theory]
    [InlineData("gfm-08e199f14ecc41d8927476e0d28c6ed4", "gfm-08e199f14ecc41d8927476e0d28c6ed4")]
    [InlineData("NL.IMBAG.NUMMERAANDUIDING.0599200000187924", "gfm-2a3472fa13b94cd189d5917bdf42491f")]
    [InlineData("0599200100024846", "gfm-ba0deefb78844d45b5f9e55f89cc384c")]
    public async Task GetAddressByGeoIdReturnSingleAddress(string address, string expected)
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/address/{address}");
        var returnObject = await response.Content.ReadFromJsonAsync<Address>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal(expected, returnObject.Id);
        Assert.True(response.Headers.CacheControl?.Public);
    }

    [Theory]
    [InlineData(OrganizationRole.Superuser)]
    [InlineData(OrganizationRole.Verifier)]
    [InlineData(OrganizationRole.Writer)]
    public async Task GetAddressByIdReturnOk(OrganizationRole role)
    {
        // Arrange
        using var client = Factory.CreateClient(role);

        // Act
        var response = await client.GetAsync($"api/address/gfm-b2a766e060444d3487c30d8de51fa408");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Headers.CacheControl?.Public);
    }
}
