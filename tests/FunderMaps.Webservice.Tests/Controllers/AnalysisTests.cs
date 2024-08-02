using FunderMaps.Core.Types.Products;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace FunderMaps.Webservice.Tests.Controllers;

/// <summary>
///     Integration test for the analysis controller.
/// </summary>
public class AnalysisTests(FunderMapsWebApplicationFactory<Program> factory) : IClassFixture<FunderMapsWebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("NL.IMBAG.PAND.0599100000685769")]
    [InlineData("0599100000685769")]
    [InlineData("NL.IMBAG.NUMMERAANDUIDING.0599200000499204")]
    [InlineData("0599200000499204")]
    public async Task AuthKeyGetProductByIdReturnProduct(string address)
    {
        using var client = factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis/{address}");
        request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.k0hEiTT0vDBvEqFHItz6wg0U6ejxceDW");

        var response = await client.SendAsync(request);
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-4f5e73d478ff452b86023a06e5b8d834", returnObject.BuildingId);
        Assert.Equal("NL.IMBAG.PAND.0599100000685769", returnObject.ExternalBuildingId);
    }

    [Fact]
    public async Task AuthKeyGetProductByIdReturnForbiddenProduct()
    {
        using var client = factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis/gfm-4f5e73d478ff452b86023a06e5b8d834");
        request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.a1LKIR7nUT8SPELGdCNnT2ngQV8RDQXI");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData("sdf-sd3kjhr834dhfjdeh")]
    [InlineData("343545435_4928374hfdkjsfh")]
    [InlineData("thisismyquerystringyes")]
    [InlineData("fdshjbf438gi")]
    [InlineData("gfm-dfcdbbabf1de41c38597c049b0cce5d4")]
    [InlineData("gfm-1437da5c31e944dd8d362264041d067a")]
    [InlineData("gfm-4f5e73d478ff452b86023a06e5b8d834")]
    public async Task GetByIdInvalidAddressThrows(string address)
    {
        using var client = factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis/{address}");
        request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.k0hEiTT0vDBvEqFHItz6wg0U6ejxceDW");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
