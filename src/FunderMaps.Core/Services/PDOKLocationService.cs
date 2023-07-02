using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Services;

public struct PDOKAddressSuggestion
{
    public string id { get; set; }
    public string type { get; set; }
    public float score { get; set; }
    public string nummeraanduiding_id { get; set; }
    public string weergavenaam { get; set; }
}

struct PDOKResponse
{
    public int numFound { get; set; }
    public int start { get; set; }
    public float maxScore { get; set; }
    public List<PDOKAddressSuggestion> docs { get; set; }
}

struct LocationServerResult
{
    public PDOKResponse response { get; set; }
}

public class PDOKLocationService
{
    /// <summary>
    ///     Default base URL for the remote service.
    /// </summary>
    private const string DefaultBaseUrl = @"https://api.pdok.nl/bzk/locatieserver/search/v3_1/";

    private readonly ILogger<PDOKLocationService> _logger;

    private HttpClient httpClient = new();

    public PDOKLocationService(ILogger<PDOKLocationService> logger)
    {
        httpClient.BaseAddress = new Uri(DefaultBaseUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Suggest addresses based on a query.
    /// </summary>
    public async Task<List<PDOKAddressSuggestion>> SuggestAsync(string query, int rows = 10)
    {
        var response = await httpClient.GetAsync($"free?fq=type:adres&fl=id,type,score,nummeraanduiding_id,weergavenaam&q={query}&rows={rows}");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Location server API call failed with status code {StatusCode}", response.StatusCode);

            throw new HttpRequestException("Location server API call failed.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<LocationServerResult>(jsonResponse);

        return responseObject.response.docs;
    }
}
