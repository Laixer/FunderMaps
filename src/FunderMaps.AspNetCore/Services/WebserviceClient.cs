using System.Net.Http.Headers;
using System.Net.Http.Json;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.AspNetCore.Services;

/// <summary>
///     Options for the open AI service.
/// </summary>
public sealed record FunderMapsWebserviceOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "FunderMapsWebservice";

    /// <summary>
    ///     FunderMaps base URL.
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    ///     FunderMaps email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     FunderMaps password.
    /// </summary>
    public string? Password { get; set; }
}

/// <summary>
///     Webservice client.
/// </summary>
public class WebserviceClient : IDisposable
{
    /// <summary>
    ///     Default base URL for the remote service.
    /// </summary>
    private const string DefaultBaseUrl = @"https://ws.fundermaps.com";

    private readonly HttpClient httpClient = new();
    private readonly FunderMapsWebserviceOptions _options;
    private readonly ILogger<WebserviceClient> _logger;

    public bool IsAuthenticated => httpClient.DefaultRequestHeaders.Authorization is not null;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public WebserviceClient(IOptions<FunderMapsWebserviceOptions> options, ILogger<WebserviceClient> logger)
    {
        httpClient.BaseAddress = new Uri(options?.Value.BaseUrl ?? DefaultBaseUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
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

        var response = await httpClient.PostAsJsonAsync("auth/signin", new
        {
            email = _options.Email,
            password = _options.Password,
        });

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("FunderMaps API call failed with status code {StatusCode}", response.StatusCode);

            throw new HttpRequestException("FunderMaps API call failed");
        }

        var authToken = await response.Content.ReadFromJsonAsync<SignInSecurityTokenDto>();
        if (authToken is null)
        {
            throw new HttpRequestException("FunderMaps API call failed");
        }

        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", authToken.Token);
    }

    /// <summary>
    ///     Get analysis product from webservice.
    /// </summary>
    /// <param name="id">Object identifier.</param>
    public async Task<AnalysisProduct?> GetAnalysisAsync(string id)
    {
        await EnsureAuthenticationAsync();
        return await httpClient.GetFromJsonAsync<AnalysisProduct>($"api/v3/product/analysis/{id}");
    }

    /// <summary>
    ///     Get statistics product from webservice.
    /// </summary>
    /// <param name="id">Object identifier.</param>
    public async Task<StatisticsProduct?> GetStatisticsAsync(string id)
    {
        await EnsureAuthenticationAsync();
        return await httpClient.GetFromJsonAsync<StatisticsProduct>($"api/v3/product/statistics/{id}");
    }

    /// <summary>
    ///     Free managed resources.
    /// </summary>
    public void Dispose() => httpClient.Dispose();
}
