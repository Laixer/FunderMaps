using System.Net;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FunderMaps.Webservice.Tests;

public class VersionsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetVersionAuthorizedReturnSuccessAndCorrectContentType()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("api/version");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
