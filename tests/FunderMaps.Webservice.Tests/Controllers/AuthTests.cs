using Bogus;
using Bogus.DataSets;
using FunderMaps.Core.DataTransferObjects;
using System.Net;
using Xunit;

namespace FunderMaps.Webservice.Tests.Controllers;

/// <summary>
///     Tests our authentication.
/// </summary>
public class AuthTests(FunderMapsWebApplicationFactory<Program> factory) : IClassFixture<FunderMapsWebApplicationFactory<Program>>
{
    [Fact]
    public async Task SignInReturnSuccessAndToken()
    {
        // await TestStub.LoginAsync(Factory, "Javier40@yahoo.com", "fundermaps");
        // await TestStub.LoginAsync(Factory, "Freda@contoso.com", "fundermaps");
        // await TestStub.LoginAsync(Factory, "patsy@contoso.com", "fundermaps");
        // await TestStub.LoginAsync(Factory, "lester@contoso.com", "fundermaps");

        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/auth/signin", new SignInDto()
        {
            Email = "Javier40@yahoo.com",
            Password = "fundermaps",
        });
        var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.NotNull(returnObject.Id);
        Assert.NotNull(returnObject.Token);
        Assert.NotNull(returnObject.Issuer);
        Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
    }

    // [Fact]
    // public async Task RefreshSignInReturnSuccessAndToken()
    // {
    //     using var client = factory.CreateClient();

    //     var response = await client.GetAsync("api/auth/token-refresh");
    //     var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

    //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //     Assert.NotNull(returnObject);
    //     Assert.NotNull(returnObject.Id);
    //     Assert.NotNull(returnObject.Token);
    //     Assert.NotNull(returnObject.Issuer);
    //     Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
    // }

    [Fact]
    public async Task SignInInvalidCredentialsReturnError()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/auth/signin", new SignInDto
        {
            Email = "lester@contoso.com",
            Password = new Randomizer().Password(64),
        });
        // var returnObject = await response.Content.ReadFromJsonAsync<ProblemModel>();

        Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);

        // Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        // Assert.NotNull(returnObject);
        // Assert.Equal((short)HttpStatusCode.Unauthorized, returnObject.Status);
        // Assert.Contains("Login", returnObject.Title, StringComparison.InvariantCultureIgnoreCase);
    }

    [Theory]
    [InlineData("test@test.com", "")]
    [InlineData("", "password")]
    [InlineData("", "")]
    public async Task SignInInvalidRequestReturnBadRequest(string email, string password)
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/auth/signin", new SignInDto
        {
            Email = email,
            Password = password,
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // [Theory]
    // [InlineData("api/user")]
    // [InlineData("api/auth/token-refresh")]
    // public async Task RefreshSignInReturnUnauthorized(string uri)
    // {
    //     using var client = factory.CreateClient();

    //     var response = await client.GetAsync(uri);

    //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    // }
}
