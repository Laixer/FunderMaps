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
public sealed record FunderMapsClientOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "FunderMaps";

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

    /// <summary>
    ///     FunderMaps API key.
    /// </summary>
    public string? ApiKey { get; set; }
}

/// <summary>
///     Webservice client.
/// </summary>
public class FunderMapsClient : IDisposable
{
    /// <summary>
    ///     Default base URL for the remote service.
    /// </summary>
    private const string DefaultBaseUrl = @"https://ws.fundermaps.com";

    private readonly HttpClient httpClient = new();
    private readonly FunderMapsClientOptions _options;
    private readonly ILogger<WebserviceClient> _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public FunderMapsClient(IOptions<FunderMapsClientOptions> options, ILogger<WebserviceClient> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;

        httpClient.BaseAddress = new Uri(_options.BaseUrl ?? DefaultBaseUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (_options.ApiKey is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", options.Value.ApiKey);
        }
        else
        {
            var authToken = GetAuthenticationTokenAsync(
                _options.Email ?? throw new ArgumentNullException(nameof(_options.Email)),
                _options.Password ?? throw new ArgumentNullException(nameof(_options.Password))
                ).Result;

            _options.ApiKey = authToken.Token;
            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", _options.ApiKey);
        }
    }

    /// <summary>
    ///     Ensures there is a valid authentication token.
    /// </summary>
    private async Task<SignInSecurityTokenDto> GetAuthenticationTokenAsync(string email, string password)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/signin", new
        {
            email = email,
            password = password,
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

        return authToken;
    }

    /// <summary>
    ///     Get analysis product from webservice.
    /// </summary>
    /// <param name="id">Object identifier.</param>
    public Task<AnalysisProduct?> GetAnalysisAsync(string id)
        => httpClient.GetFromJsonAsync<AnalysisProduct>($"api/v3/product/analysis/{id}");

    /// <summary>
    ///     Get statistics product from webservice.
    /// </summary>
    /// <param name="id">Object identifier.</param>
    public Task<StatisticsProduct?> GetStatisticsAsync(string id)
        => httpClient.GetFromJsonAsync<StatisticsProduct>($"api/v3/product/statistics/{id}");

    /// <summary>
    ///     Free managed resources.
    /// </summary>
    public void Dispose() => httpClient.Dispose();
}
