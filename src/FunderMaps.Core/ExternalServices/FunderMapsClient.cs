using System.Net.Http.Headers;
using System.Net.Http.Json;
using FunderMaps.Core.Options;
using FunderMaps.Core.Types.Products;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Core.ExternalServices;

/// <summary>
///     Webservice client.
/// </summary>
public class FunderMapsClient : IDisposable
{
    /// <summary>
    ///     Default base URL for the remote service.
    /// </summary>
    private const string DefaultBaseUrl = "https://ws.fundermaps.com";

    private readonly HttpClient httpClient = new();
    private readonly FunderMapsOptions _options;
    private readonly ILogger<FunderMapsClient> _logger;

    // FUTURE: This is copied from AspNetCore. We should use the same.
    /// <summary>
    ///     User signin result DTO.
    /// </summary>
    public record SignInSecurityToken
    {
        /// <summary>
        ///     Authentication token identifier.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        ///     Authentication issuer.
        /// </summary>
        public string? Issuer { get; init; }

        /// <summary>
        ///     Authentication token.
        /// </summary>
        public string? Token { get; init; }

        /// <summary>
        ///     Authentication token valid from datetime.
        /// </summary>
        public DateTime ValidFrom { get; init; }

        /// <summary>
        ///     Authentication token valid until datetime.
        /// </summary>
        public DateTime ValidTo { get; init; }
    }

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public FunderMapsClient(IOptions<FunderMapsOptions> options, ILogger<FunderMapsClient> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;

        var host = new Uri(_options.BaseUrl ?? DefaultBaseUrl);

        _logger.LogDebug("Using FunderMaps API at {host}", host);

        httpClient.BaseAddress = new Uri(host.Scheme + "://" + host.Host + (host.IsDefaultPort ? "" : ":" + host.Port));
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (_options.ApiKey is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new("AuthKey", options.Value.ApiKey);
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
    private async Task<SignInSecurityToken> GetAuthenticationTokenAsync(string email, string password)
    {
        _logger.LogDebug("Requesting authentication token");

        var response = await httpClient.PostAsJsonAsync("api/auth/signin", new
        {
            email,
            password,
        });

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("FunderMaps API call failed with status code {StatusCode}", response.StatusCode);

            throw new HttpRequestException("FunderMaps API call failed");
        }

        var authToken = await response.Content.ReadFromJsonAsync<SignInSecurityToken>() ?? throw new HttpRequestException("FunderMaps API call failed");
        _logger.LogDebug("Received authentication token");

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
