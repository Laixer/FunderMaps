using System.Net.Http.Headers;
using System.Net.Http.Json;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types.Products;

namespace FunderMaps.AspNetCore.Services;

/// <summary>
///     Authentication parameters.
/// </summary>
public record Authentication
{
    /// <summary>
    ///     Email address.
    /// </summary>
    public string Email { get; init; } = default!;

    /// <summary>
    ///     Password.
    /// </summary>
    public string Password { get; init; } = default!;
}

/// <summary>
///     Webservice client.
/// </summary>
public class WebserviceClient
{
    private const string DefaultBaseUrl = @"https://ws.fundermaps.com";

    private HttpClient client = new();

    public bool IsAuthenticated => client.DefaultRequestHeaders.Authorization is not null;

    public Authentication Authentication { get; init; }

    /// <summary>
    ///     Construct a new webservice client.
    /// </summary>
    /// <param name="authentication">Authentication parameters.</param>
    /// <param name="baseUrl">Optional base URL.</param>
    public WebserviceClient(Authentication authentication, string? baseUrl = DefaultBaseUrl)
    {
        Authentication = authentication;

        if (baseUrl is null)
        {
            baseUrl = DefaultBaseUrl;
        }

        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    ///     Ensures there is a valid authentication token.
    /// </summary>
    private async Task EnsureAuthenticationAsync()
    {
        if (IsAuthenticated)
        {
            return;
        }

        var response = await client.PostAsJsonAsync("auth/signin", new
        {
            email = Authentication.Email,
            password = Authentication.Password,
        });

        var authToken = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();
        if (authToken is null)
        {
            throw new Exception(); // TODO
        }

        client.DefaultRequestHeaders.Authorization = new("Bearer", authToken.Token);
    }

    /// <summary>
    ///     Get analysis product from webservice.
    /// </summary>
    /// <param name="id">Object identifier.</param>
    public async Task<AnalysisProduct?> GetAnalysisAsync(string id)
    {
        await EnsureAuthenticationAsync();
        return await client.GetFromJsonAsync<AnalysisProduct>($"api/v3/product/analysis/{id}");
    }

    /// <summary>
    ///     Get statistics product from webservice.
    /// </summary>
    /// <param name="id">Object identifier.</param>
    public async Task<StatisticsProduct?> GetStatisticsAsync(string id)
    {
        await EnsureAuthenticationAsync();
        return await client.GetFromJsonAsync<StatisticsProduct>($"api/v3/product/statistics/{id}");
    }

    /// <summary>
    ///     Free managed resources.
    /// </summary>
    public void Dispose() => client.Dispose();
}
