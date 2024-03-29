﻿using Bogus;
using Bogus.DataSets;
using FunderMaps.Core.DataTransferObjects;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace FunderMaps.Webservice.Tests.Controllers;

/// <summary>
///     Tests our authentication.
/// </summary>
public class AuthTests(FunderMapsWebApplicationFactory<Program> factory) : IClassFixture<FunderMapsWebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("admin@fundermaps.com")]
    [InlineData("Javier40@yahoo.com")]
    [InlineData("Freda@contoso.com")]
    [InlineData("patsy@contoso.com")]
    [InlineData("lester@contoso.com")]
    [InlineData("corene@contoso.com")]
    [InlineData("PATSY@CONTOSO.COM")]
    public async Task SignInReturnSuccessAndToken(string email)
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/auth/signin", new SignInDto
        {
            Email = email,
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

    [Fact]
    public async Task RefreshSignInReturnSuccessAndToken()
    {
        using var client = factory.CreateClient();

        var authResponse = await client.PostAsJsonAsync("api/auth/signin", new SignInDto
        {
            Email = "lester@contoso.com",
            Password = "fundermaps",
        });
        var returnToken = await authResponse.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.NotNull(returnToken);

        var request = new HttpRequestMessage(HttpMethod.Get, "api/auth/token-refresh");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", returnToken.Token);

        var response = await client.SendAsync(request);
        var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.NotNull(returnObject.Id);
        Assert.NotNull(returnObject.Token);
        Assert.NotNull(returnObject.Issuer);
        Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
    }

    [Fact]
    public async Task RefreshAuthKeyReturnSuccessAndToken()
    {
        using var client = factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "api/auth/token-refresh");
        request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.a1LKIR7nUT8SPELGdCNnT2ngQV8RDQXI");

        var response = await client.SendAsync(request);
        var returnObject = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnObject);
        Assert.NotNull(returnObject.Id);
        Assert.NotNull(returnObject.Token);
        Assert.NotNull(returnObject.Issuer);
        Assert.True(returnObject.ValidTo > returnObject.ValidFrom);
    }

    [Fact]
    public async Task SignInInvalidCredentialsReturnError()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/auth/signin", new SignInDto
        {
            Email = "lester@contoso.com",
            Password = new Randomizer().Password(64),
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RefreshSignInReturnUnauthorized()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("api/auth/token-refresh");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
