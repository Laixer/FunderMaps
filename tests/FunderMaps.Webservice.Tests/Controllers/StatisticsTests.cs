using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace FunderMaps.Webservice.Tests.Controllers;

/// <summary>
///     Integration test for the statistics controller.
/// </summary>
public class StatisticsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{

    [Fact(Skip = "Needs FIX")]
    public async Task GetProductByIdReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("api/v3/product/statistics/gfm-6aae47cb5aa4416abdf19d98ba8218ac");
        var returnObject = await response.Content.ReadFromJsonAsync<StatisticsProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact(Skip = "Needs FIX")]
    public async Task GetProductByExternalIdReturnProduct()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("api/v3/product/statistics/BU05031403");
        var returnObject = await response.Content.ReadFromJsonAsync<StatisticsProduct>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory(Skip = "Needs FIX")]
    [InlineData("id=3kjhr834dhfjdeh")]
    [InlineData("bagid=4928374hfdkjsfh")]
    [InlineData("query=thisismyquerystringyes")]
    [InlineData("fdshjbf438gi")]
    public async Task GetByIdInvalidAddressThrows(string address)
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"api/v3/product/statistics/{address}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
