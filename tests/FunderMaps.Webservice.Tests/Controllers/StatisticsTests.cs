using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Types.Products;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace FunderMaps.Webservice.Tests.Controllers;

/// <summary>
///     Integration test for the statistics controller.
/// </summary>
public class StatisticsTests(FunderMapsWebApplicationFactory<Program> factory) : IClassFixture<FunderMapsWebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("gfm-7bc9bb6497984a13a2cc95ea1a284825")]
    [InlineData("BU05990324")]
    [InlineData("NL.IMBAG.PAND.0599100000685769")]
    [InlineData("NL.IMBAG.NUMMERAANDUIDING.0599200000499204")]
    public async Task AuthKeyGetProductByIdReturnProduct(string address)
    {
        using var client = factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/statistics/{address}");
        request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.k0hEiTT0vDBvEqFHItz6wg0U6ejxceDW");

        var response = await client.SendAsync(request);
        var returnObject = await response.Content.ReadFromJsonAsync<StatisticsProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
    }

    [Fact]
    public async Task AuthKeyGetProductByIdReturnForbiddenProduct()
    {
        using var client = factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/statistics/gfm-4f5e73d478ff452b86023a06e5b8d834");
        request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.a1LKIR7nUT8SPELGdCNnT2ngQV8RDQXI");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData("sdf-sd3kjhr834dhfjdeh")]
    [InlineData("343545435_4928374hfdkjsfh")]
    [InlineData("thisismyquerystringyes")]
    [InlineData("fdshjbf438gi")]
    public async Task GetByIdInvalidAddressThrows(string address)
    {
        using var client = factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/statistics/{address}");
        request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.k0hEiTT0vDBvEqFHItz6wg0U6ejxceDW");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
