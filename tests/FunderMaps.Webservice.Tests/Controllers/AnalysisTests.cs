using FunderMaps.Core.Types.Products;
using System.Net;
using Xunit;

namespace FunderMaps.Webservice.Tests.Controllers;

/// <summary>
///     Integration test for the analysis controller.
/// </summary>
public class AnalysisTests(FunderMapsWebApplicationFactory<Program> factory) : IClassFixture<FunderMapsWebApplicationFactory<Program>>
{
    [Fact(Skip = "Needs FIX")]
    public async Task GetProductByIdReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"api/v3/product/analysis?id=gfm-4f5e73d478ff452b86023a06e5b8d834");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("NL.IMBAG.PAND.0599100000685769", returnObject.ExternalBuildingId);
    }

    [Fact(Skip = "Needs FIX")]
    public async Task GetProductByExternalIdReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"api/v3/product/analysis?id=NL.IMBAG.PAND.0599100000661262");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-39bd02bbc79e4ed08c97fd6afbbf5fee", returnObject.BuildingId);
    }

    [Fact(Skip = "Needs FIX")]
    public async Task GetProductByExternalIdBag1ReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"api/v3/product/analysis?id=0599100000630926");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-d6cc2bda840249209291b125174c07fc", returnObject.BuildingId);
    }

    [Fact(Skip = "Needs FIX")]
    public async Task GetProductByExternalAddressIdReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"api/v3/product/analysis?id=NL.IMBAG.NUMMERAANDUIDING.0599200000308423");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-21621a43af364bdb86f192201473ccf9", returnObject.BuildingId);
    }

    [Fact(Skip = "Needs FIX")]
    public async Task GetProductByExternalAddressIdBag1ReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"api/v3/product/analysis?id=0599200000337325");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-a724269605954e9285ca378b77dafcda", returnObject.BuildingId);
    }

    [Fact(Skip = "Needs FIX")]
    public async Task GetRiskIndexByIdReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"api/v3/product/at_risk?id=gfm-1eec772e31634092bc4c3f0cf18e38b8");
        var returnObject = await response.Content.ReadFromJsonAsync<bool>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(returnObject);
    }

    [Fact(Skip = "Needs FIX")]
    public async Task GetRiskIndexByExternalIdReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"api/v3/product/at_risk?id=NL.IMBAG.PAND.0599100000669737");
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
