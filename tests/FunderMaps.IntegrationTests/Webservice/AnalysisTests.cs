using FunderMaps.Core.Types.Products;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice;

/// <summary>
///     Integration test for the analysis controller.
/// </summary>
public class AnalysisTests(WebserviceFixtureFactory factory) : IClassFixture<WebserviceFixtureFactory>
{
    [Fact]
    public async Task GetProductByIdReturnProduct()
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=gfm-4f5e73d478ff452b86023a06e5b8d834");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("NL.IMBAG.PAND.0599100000685769", returnObject.ExternalBuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetProductByExternalIdReturnProduct()
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=NL.IMBAG.PAND.0599100000661262");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-39bd02bbc79e4ed08c97fd6afbbf5fee", returnObject.BuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetProductByExternalIdBag1ReturnProduct()
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=0599100000630926");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-d6cc2bda840249209291b125174c07fc", returnObject.BuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetProductByExternalAddressIdReturnProduct()
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=NL.IMBAG.NUMMERAANDUIDING.0599200000308423");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-21621a43af364bdb86f192201473ccf9", returnObject.BuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetProductByExternalAddressIdBag1ReturnProduct()
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id=0599200000337325");
        var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.Equal("gfm-a724269605954e9285ca378b77dafcda", returnObject.BuildingId);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetRiskIndexByIdReturnProduct()
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/at_risk?id=gfm-1eec772e31634092bc4c3f0cf18e38b8");
        var returnObject = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(returnObject);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(factory, "analysis3") > 0);
    }

    [Fact]
    public async Task GetRiskIndexByExternalIdReturnProduct()
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/at_risk?id=NL.IMBAG.PAND.0599100000669737");
        var returnObject = await response.Content.ReadFromJsonAsync<bool>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(returnObject);

        Assert.True(await WebserviceStub.CheckQuotaUsageAsync(factory, "analysis3") > 0);
    }

    [Theory]
    [InlineData("id=3kjhr834dhfjdeh")]
    [InlineData("bagid=4928374hfdkjsfh")]
    [InlineData("query=thisismyquerystringyes")]
    [InlineData("fdshjbf438gi")]
    public async Task GetRiskIndexByExternalIdInvalidAddressThrows(string address)
    {
        // Arrange
        using var client = factory.CreateClient();

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
        using var client = factory.CreateClient();

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
        using var client = factory.CreateClient();

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
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v3/product/analysis?id={address}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
