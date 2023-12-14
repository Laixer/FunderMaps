using System.Net;
using Xunit;

namespace FunderMaps.Webservice.Tests.Controllers;

public class VersionsTests(FunderMapsWebApplicationFactory<Program> factory) : IClassFixture<FunderMapsWebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetVersionAuthorizedReturnSuccessAndCorrectContentType()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("version");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
