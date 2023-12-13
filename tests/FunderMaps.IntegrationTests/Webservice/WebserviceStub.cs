using FunderMaps.Core.Entities;
using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice;

/// <summary>
///     Teststub for all webservice tests.
/// </summary>
public static class WebserviceStub
{
    public static async Task<int> CheckQuotaUsageAsync(WebserviceFixtureFactory factory, string product)
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/quota/usage");
        var returnObject = await response.Content.ReadFromJsonAsync<IList<ProductTelemetry>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);

        return returnObject.Where(p => p.Product == product).Select(p => p.Count).First();
    }
}
