using FunderMaps.Core.DataTransferObjects;
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
    // TODO: Add NL.IMBAG.NUMMERAANDUIDING.0599200000308423
    // TODO: Add 0599200000337325
    [Theory]
    [InlineData("gfm-4f5e73d478ff452b86023a06e5b8d834")]
    [InlineData("NL.IMBAG.PAND.0599100000685769")]
    [InlineData("0599100000685769")]
    public async Task GetProductByIdReturnProduct(string address)
    {
        using var client = factory.CreateClient();

        var authResponse = await client.PostAsJsonAsync("api/auth/signin", new SignInDto()
        {
            Email = "lester@contoso.com",
            Password = "fundermaps",
        });
        var returnToken = await authResponse.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.NotNull(returnToken);

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis?id={address}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", returnToken.Token);

        var response = await client.SendAsync(request);
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-4f5e73d478ff452b86023a06e5b8d834", returnObject.BuildingId);
        Assert.Equal("NL.IMBAG.PAND.0599100000685769", returnObject.ExternalBuildingId);
    }

    // TODO: Add NL.IMBAG.NUMMERAANDUIDING.0599200000308423
    // TODO: Add 0599200000337325
    [Theory]
    [InlineData("gfm-4f5e73d478ff452b86023a06e5b8d834")]
    [InlineData("NL.IMBAG.PAND.0599100000685769")]
    [InlineData("0599100000685769")]
    public async Task GetRiskIndexByIdReturnProduct(string address)
    {
        using var client = factory.CreateClient();

        var authResponse = await client.PostAsJsonAsync("api/auth/signin", new SignInDto()
        {
            Email = "lester@contoso.com",
            Password = "fundermaps",
        });
        var returnToken = await authResponse.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.NotNull(returnToken);

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/at_risk?id={address}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", returnToken.Token);

        var response = await client.SendAsync(request);
        var returnObject = await response.Content.ReadFromJsonAsync<bool>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(returnObject);
    }

    // [Theory]
    // [InlineData("id=3kjhr834dhfjdeh")]
    // [InlineData("bagid=4928374hfdkjsfh")]
    // [InlineData("query=thisismyquerystringyes")]
    // [InlineData("fdshjbf438gi")]
    // public async Task GetRiskIndexByExternalIdInvalidAddressThrows(string address)
    // {
    //     using var client = factory.CreateClient();

    //     var response = await client.GetAsync($"api/v3/product/at_risk?id={address}");

    //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    // }

    // [Theory]
    // [InlineData("gfm-bc31bec346744745b29f8505dff8182f")]
    // [InlineData("gfm-00096758461b4c8c8a8c145790126beb")]
    // public async Task GetRiskIndexByExternalIdAddressNotFoundThrows(string address)
    // {
    //     using var client = factory.CreateClient();

    //     var response = await client.GetAsync($"api/v3/product/at_risk?id={address}");

    //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    // }

    // [Theory]
    // [InlineData("id=3kjhr834dhfjdeh")]
    // [InlineData("bagid=4928374hfdkjsfh")]
    // [InlineData("query=thisismyquerystringyes")]
    // [InlineData("fdshjbf438gi")]
    // public async Task GetByIdInvalidAddressThrows(string address)
    // {
    //     using var client = factory.CreateClient();

    //     var response = await client.GetAsync($"api/v3/product/analysis?id={address}");

    //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    // }

    // [Theory]
    // [InlineData("gfm-dfcdbbabf1de41c38597c049b0cce5d4")]
    // [InlineData("gfm-1437da5c31e944dd8d362264041d067a")]
    // public async Task GetByIdAddressNotFoundThrows(string address)
    // {
    //     using var client = factory.CreateClient();

    //     var response = await client.GetAsync($"api/v3/product/analysis?id={address}");

    //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    // }
}
