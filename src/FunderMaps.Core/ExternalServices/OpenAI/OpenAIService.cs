using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Core.ExternalServices.OpenAI;

// TODO: Implement OpenAI interface.
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
        public int TextCharacters { get; set; }
        public int TextUnits { get; set; }
        public int TotalCharacters { get; set; }
        public int TotalUnits { get; set; }
    }

    private class OpenAIResponseChoice
    {
        public string? Text { get; set; }
        public int Index { get; set; }
        public List<double> Logprobs { get; set; } = new();
        public string? FinishReason { get; set; }
    }

    struct OpenAIResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Created { get; set; }
        public string Model { get; set; }
        public List<OpenAIResponseChoice> Choices { get; set; }
        public OpenAIResponseUsage Usage { get; set; }
    }

    struct OpenAIRequest
    {
        public string Prompt { get; set; }
        public int MaxTokens { get; set; }
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

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<OpenAIResponse>(jsonResponse, options);

        return responseObject.Choices[0].Text;
    }

    /// <summary>
    ///     Free managed resources.
    /// </summary>
    public void Dispose() => httpClient.Dispose();
}