using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Types.Products;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace FunderMaps.Webservice.Tests.Controllers;

/// <summary>
///     Integration test for the analysis controller.
/// </summary>
public class AnalysisTests(FunderMapsWebApplicationFactory<Program> factory) : IClassFixture<FunderMapsWebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("gfm-4f5e73d478ff452b86023a06e5b8d834")]
    [InlineData("NL.IMBAG.PAND.0599100000685769")]
    [InlineData("0599100000685769")]
    [InlineData("NL.IMBAG.NUMMERAANDUIDING.0599200000499204")]
    [InlineData("0599200000499204")]
    public async Task GetProductByIdReturnProduct(string address)
    {
        using var client = factory.CreateClient();

        var authResponse = await client.PostAsJsonAsync("api/auth/signin", new SignInDto()
        {
            Email = "lester@contoso.com",
            Password = "fundermaps",
        });
        var returnToken = await authResponse.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.NotNull(returnToken);

        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis?id={address}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", returnToken.Token);

            var response = await client.SendAsync(request);
            var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.Equal("gfm-4f5e73d478ff452b86023a06e5b8d834", returnObject.BuildingId);
            Assert.Equal("NL.IMBAG.PAND.0599100000685769", returnObject.ExternalBuildingId);
        }

        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis/{address}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", returnToken.Token);

            var response = await client.SendAsync(request);
            var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.Equal("gfm-4f5e73d478ff452b86023a06e5b8d834", returnObject.BuildingId);
            Assert.Equal("NL.IMBAG.PAND.0599100000685769", returnObject.ExternalBuildingId);
        }
    }

    [Theory]
    [InlineData("gfm-4f5e73d478ff452b86023a06e5b8d834")]
    [InlineData("NL.IMBAG.PAND.0599100000685769")]
    [InlineData("0599100000685769")]
    [InlineData("NL.IMBAG.NUMMERAANDUIDING.0599200000499204")]
    [InlineData("0599200000499204")]
    public async Task AuthKeyGetProductByIdReturnProduct(string address)
    {
        using var client = factory.CreateClient();

        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis?id={address}");
            request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.a1LKIR7nUT8SPELGdCNnT2ngQV8RDQXI");

            var response = await client.SendAsync(request);
            var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.Equal("gfm-4f5e73d478ff452b86023a06e5b8d834", returnObject.BuildingId);
            Assert.Equal("NL.IMBAG.PAND.0599100000685769", returnObject.ExternalBuildingId);
        }

        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis/{address}");
            request.Headers.Authorization = new AuthenticationHeaderValue("AuthKey", "fmsk.a1LKIR7nUT8SPELGdCNnT2ngQV8RDQXI");

            var response = await client.SendAsync(request);
            var returnObject = await response.Content.ReadFromJsonAsync<AnalysisProduct>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnObject);
            Assert.Equal("gfm-4f5e73d478ff452b86023a06e5b8d834", returnObject.BuildingId);
            Assert.Equal("NL.IMBAG.PAND.0599100000685769", returnObject.ExternalBuildingId);
        }
    }

    [Theory]
    [InlineData("gfm-4f5e73d478ff452b86023a06e5b8d834")]
    [InlineData("NL.IMBAG.PAND.0599100000685769")]
    [InlineData("0599100000685769")]
    [InlineData("NL.IMBAG.NUMMERAANDUIDING.0599200000499204")]
    [InlineData("0599200000499204")]
    public async Task GetRiskIndexByIdReturnProduct(string address)
    {
        using var client = factory.CreateClient();

        var authResponse = await client.PostAsJsonAsync("api/auth/signin", new SignInDto()
        {
            Email = "lester@contoso.com",
            Password = "fundermaps",
        });
        var returnToken = await authResponse.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.NotNull(returnToken);

        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/at_risk?id={address}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", returnToken.Token);

            var response = await client.SendAsync(request);
            var returnObject = await response.Content.ReadFromJsonAsync<bool>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnObject);
        }

        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/at_risk/{address}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", returnToken.Token);

            var response = await client.SendAsync(request);
            var returnObject = await response.Content.ReadFromJsonAsync<bool>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(returnObject);
        }
    }

    [Theory]
    [InlineData("sdf-sd3kjhr834dhfjdeh")]
    [InlineData("343545435_4928374hfdkjsfh")]
    [InlineData("thisismyquerystringyes")]
    [InlineData("fdshjbf438gi")]
    // [InlineData("gfm-dfcdbbabf1de41c38597c049b0cce5d4")]
    // [InlineData("gfm-1437da5c31e944dd8d362264041d067a")]
    public async Task GetByIdInvalidAddressThrows(string address)
    {
        using var client = factory.CreateClient();

        var authResponse = await client.PostAsJsonAsync("api/auth/signin", new SignInDto()
        {
            Email = "lester@contoso.com",
            Password = "fundermaps",
        });
        var returnToken = await authResponse.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();

        Assert.NotNull(returnToken);

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/v3/product/analysis?id={address}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", returnToken.Token);

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
