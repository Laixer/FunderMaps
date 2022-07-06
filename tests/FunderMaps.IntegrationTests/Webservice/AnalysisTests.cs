using FunderMaps.Core.Types.Products;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice;

/// <summary>
///     Integration test for the analysis controller.
/// </summary>
public class AnalysisTests : IClassFixture<WebserviceFixtureFactory>
{
    private WebserviceFixtureFactory Factory { get; }

    /// <summary>
    ///     Create new instance and setup the test data.
    /// </summary>
    public AnalysisTests(WebserviceFixtureFactory factory)
        => Factory = factory;

    [Fact]
    public async Task GetProductByIdReturnProduct()
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=gfm-ac31bec346744745b29f8505dff8182e");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct3>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("NL.IMBAG.LIGPLAATS.0503020000111954", returnObject.ExternalBuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetProductByExternalIdReturnProduct()
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=NL.IMBAG.LIGPLAATS.0503020000111954");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct3>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("gfm-ac31bec346744745b29f8505dff8182e", returnObject.BuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetProductByExternalIdBag1ReturnProduct()
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=0503020000111954");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct3>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("gfm-ac31bec346744745b29f8505dff8182e", returnObject.BuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetProductByExternalAddressIdReturnProduct()
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=NL.IMBAG.NUMMERAANDUIDING.0503200000111954");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct3>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("gfm-ac31bec346744745b29f8505dff8182e", returnObject.BuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetProductByExternalAddressIdBag1ReturnProduct()
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=0503200000111954");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct3>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("gfm-ac31bec346744745b29f8505dff8182e", returnObject.BuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetRiskIndexByIdReturnProduct()
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/at_risk?id=gfm-ac31bec346744745b29f8505dff8182e");
        var returnObject = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(returnObject);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetRiskIndexByExternalIdReturnProduct()
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/at_risk?id=NL.IMBAG.LIGPLAATS.0503020000111954");
        var returnObject = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(returnObject);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(Factory, "analysis3") > 0);
    }

    [Theory]
    [InlineData("id=3kjhr834dhfjdeh")]
    [InlineData("bagid=4928374hfdkjsfh")]
    [InlineData("query=thisismyquerystringyes")]
    [InlineData("fdshjbf438gi")]
    public async Task GetRiskIndexByExternalIdInvalidAddressThrows(string address)
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/at_risk?id={address}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("gfm-bc31bec346744745b29f8505dff8182f")]
    [InlineData("gfm-00096758461b4c8c8a8c145790126beb")]
    public async Task GetRiskIndexByExternalIdAddressNotFoundThrows(string address)
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/at_risk?id={address}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("id=3kjhr834dhfjdeh")]
    [InlineData("bagid=4928374hfdkjsfh")]
    [InlineData("query=thisismyquerystringyes")]
    [InlineData("fdshjbf438gi")]
    public async Task GetByIdInvalidAddressThrows(string address)
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id={address}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("gfm-dfcdbbabf1de41c38597c049b0cce5d4")]
    [InlineData("gfm-1437da5c31e944dd8d362264041d067a")]
    public async Task GetByIdAddressNotFoundThrows(string address)
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id={address}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
