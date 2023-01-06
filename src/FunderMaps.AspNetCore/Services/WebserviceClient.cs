using System.Net.Http.Headers;
using System.Net.Http.Json;
using FunderMaps.AspNetCore.DataTransferObjects;

namespace FunderMaps.AspNetCore.Services;

/// <summary>
///     Authentication parameters.
/// </summary>
public record Authentication
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

/// <summary>
///     Webservice client.
/// </summary>
public class WebserviceClient
{
    const string DefaultBaseUrl = @"https://ws.fundermaps.com/";

    HttpClient client = new();

    public bool IsAuthenticated => client.DefaultRequestHeaders.Authorization is not null;

    public Authentication Authentication { get; set; }

    /// <summary>
    ///     Construct a new webservice client.
    /// </summary>
    /// <param name="authentication">Authentication parameters.</param>
    /// <param name="baseUrl">Optional base URL.</param>
    public WebserviceClient(Authentication authentication, string baseUrl = DefaultBaseUrl)
    {
        Authentication = authentication;

        client.BaseAddress = new(baseUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    ///     Authenticate the user against the webservice.
    /// </summary>
    async Task LoginAsync()
    {
        var response = await client.PostAsJsonAsync("api/auth/signin", new
        {
            email = Authentication.Email,
            password = Authentication.Password,
        });

        var authToken = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();
        if (authToken is null)
        {
            throw new Exception();
        }

        client.DefaultRequestHeaders.Authorization = new("Bearer", authToken.Token);
    }

    public async Task<FunderMaps.Core.Types.Products.AnalysisProduct3?> GetAnalysisAsync(string id)
    {
        if (!IsAuthenticated)
        {
            await LoginAsync();
        }

        return await client.GetFromJsonAsync<FunderMaps.Core.Types.Products.AnalysisProduct3>($"api/v3/product/analysis?id={id}");
    }

    public async Task<FunderMaps.Core.Types.Products.StatisticsProduct?> GetStatisticsAsync(string id)
    {
        if (!IsAuthenticated)
        {
            await LoginAsync();
        }

        return await client.GetFromJsonAsync<FunderMaps.Core.Types.Products.StatisticsProduct>($"api/v3/product/statistics?id={id}");
    }
}
