using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Services;

public struct PDOKSuggestion
{
    public string Id { get; set; }
    public string Suggestion { get; set; }
}

public struct PDOKDocument
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
    public List<PDOKDocument> docs { get; set; }
}

struct LocationServerResult
{
    public PDOKResponse response { get; set; }
    public JsonElement highlighting { get; set; }
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
    ///     Lookup PDOK document by id.
    /// </summary>
    public async Task<PDOKDocument> LookupAsync(string id)
    {
        var response = await httpClient.GetAsync($"lookup?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Location server API call failed with status code {StatusCode}", response.StatusCode);

            throw new HttpRequestException("Location server API call failed.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<LocationServerResult>(jsonResponse);

        return responseObject.response.docs.First();
    }

    /// <summary>
    ///     Search addresses based on a query.
    /// </summary>
    public async Task<List<PDOKDocument>> SearchAsync(string query, int rows = 10)
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

    /// <summary>
    ///     Suggest addresses based on a query.
    /// </summary>
    public async Task<List<PDOKSuggestion>> SuggestAsync(string query, int rows = 10)
    {
        var response = await httpClient.GetAsync($"suggest?fq=type:adres&q={query}&rows={rows}");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Location server API call failed with status code {StatusCode}", response.StatusCode);

            throw new HttpRequestException("Location server API call failed.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<LocationServerResult>(jsonResponse);

        var suggestions = new List<PDOKSuggestion>();
        foreach (var highlightProperty in responseObject.highlighting.EnumerateObject())
        {
            var suggestionProperty = highlightProperty.Value.GetProperty("suggest");

            if (suggestionProperty.GetArrayLength() == 0)
            {
                continue;
            }

            var suggestion = suggestionProperty[0].GetString();
            if (string.IsNullOrEmpty(suggestion))
            {
                continue;
            }

            suggestions.Add(new PDOKSuggestion
            {
                Id = highlightProperty.Name,
                Suggestion = suggestion,
            });
        }

        return suggestions;
    }
}
