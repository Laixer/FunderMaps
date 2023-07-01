using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Core.Services;

/// <summary>
///     Options for the open AI service.
/// </summary>
public sealed record OpenAIOptions
{
    /// <summary>
    ///     Configuration section key.
    /// </summary>
    public const string Section = "OpenAI";

    /// <summary>
    ///     OpenAI API key.
    /// </summary>
    public string? ApiKey { get; set; }
}

public class OpenAIService : IDisposable
{
    /// <summary>
    ///     Default base URL for the remote service.
    /// </summary>
    private const string DefaultBaseUrl = @"https://api.openai.com";

    private readonly HttpClient httpClient = new();
    private readonly IOptions<OpenAIOptions> _options;
    private readonly ILogger<OpenAIService> _logger;

    private class OpenAIResponseUsage
    {
        public int text_characters { get; set; }
        public int text_units { get; set; }
        public int total_characters { get; set; }
        public int total_units { get; set; }
    }

    private class OpenAIResponseChoice
    {
        public string text { get; set; }
        public int index { get; set; }
        public List<double> logprobs { get; set; }
        public string finish_reason { get; set; }
    }

    struct OpenAIResponse
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public List<OpenAIResponseChoice> choices { get; set; }
        public OpenAIResponseUsage usage { get; set; }
    }

    struct OpenAIRequest
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }
        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }
    }

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public OpenAIService(IOptions<OpenAIOptions> options, ILogger<OpenAIService> logger)
    {
        httpClient.BaseAddress = new Uri(DefaultBaseUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", options.Value.ApiKey);

        _options = options;
        _logger = logger;
    }

    public async Task<string?> GenerateTextAsync(string prompt, int maxTokens = 100)
    {
        var requestBody = new OpenAIRequest
        {
            Prompt = prompt,
            MaxTokens = maxTokens
        };

        string jsonBody = JsonSerializer.Serialize(requestBody);
        using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("v1/engines/text-davinci-003/completions", content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("OpenAI API call failed with status code {StatusCode}", response.StatusCode);

            throw new HttpRequestException("OpenAI API call failed");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<OpenAIResponse>(jsonResponse);

        return responseObject.choices[0].text;
    }

    /// <summary>
    ///     Free managed resources.
    /// </summary>
    public void Dispose() => httpClient.Dispose();
}