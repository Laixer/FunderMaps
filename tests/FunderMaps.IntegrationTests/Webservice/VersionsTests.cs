using System.Net;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice;

public class VersionsTests : IClassFixture<WebserviceFixtureFactory>
{
    private WebserviceFixtureFactory Factory { get; }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public VersionsTests(WebserviceFixtureFactory factory)
        => Factory = factory;

    [Fact]
    public async Task GetVersionUnauthorizedReturnSuccessAndCorrectContentType()
    {
        await TestStub.VersionAsync(Factory);
    }

    [Fact]
    public async Task GetVersionAuthorizedReturnSuccessAndCorrectContentType()
    {
        // Arrange
        using var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/version");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
