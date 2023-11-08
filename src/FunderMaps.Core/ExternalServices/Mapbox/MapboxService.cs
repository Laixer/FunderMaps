using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Amazon.S3;
using Amazon;
using Amazon.S3.Model;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Core.ExternalServices.Mapbox;

struct MapboxCredentialResponse
{
    public string bucket { get; set; }
    public string key { get; set; }
    public string accessKeyId { get; set; }
    public string secretAccessKey { get; set; }
    public string sessionToken { get; set; }
    public string url { get; set; }
}

public struct MapboxUploadResponse
{
    public bool complete { get; set; }
    public string tileset { get; set; }
    public string error { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public DateTime modified { get; set; }
    public DateTime created { get; set; }
    public string owner { get; set; }
    public int progress { get; set; }
}

internal class MapboxService : IMapboxService, IDisposable
{
    /// <summary>
    ///     Default base URL for the remote service.
    /// </summary>
    private const string DefaultBaseUrl = @"https://api.mapbox.com";

    private readonly HttpClient httpClient = new();
    private readonly MapboxOptions _options;
    private readonly ILogger<MapboxService> _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public MapboxService(IOptions<MapboxOptions> options, ILogger<MapboxService> logger)
    {
        httpClient.BaseAddress = new Uri(DefaultBaseUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    private async Task<MapboxCredentialResponse> UploadCredentialAsync()
    {
        var response = await httpClient.GetAsync($"uploads/v1/{_options.Account}/credentials?access_token={_options.ApiKey}");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Mapbox API call failed with status code {StatusCode}", response.StatusCode);

            throw new HttpRequestException("Mapbox API call failed");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<MapboxCredentialResponse>(jsonResponse);

        return responseObject;
    }

    private async Task UploadFileToS3(MapboxCredentialResponse credentials, string filePath)
    {
        var clientConfig = new AmazonS3Config()
        {
            RegionEndpoint = RegionEndpoint.USEast1,
        };

        var s3Client = new AmazonS3Client(credentials.accessKeyId, credentials.secretAccessKey, credentials.sessionToken, clientConfig);

        var request = new PutObjectRequest
        {
            BucketName = credentials.bucket,
            Key = credentials.key,
            FilePath = filePath,
        };

        await s3Client.PutObjectAsync(request);
    }

    /// <summary>
    ///     Upload file to Mapbox.
    /// </summary>
    /// <param name="name">Name of the file.</param>
    /// <param name="tileset">Tileset name.</param>
    /// <param name="filePath">Path to file.</param>
    public async Task UploadAsync(string name, string tileset, string filePath)
    {
        var credentials = await UploadCredentialAsync();

        await UploadFileToS3(credentials, filePath);

        var requestBody = new
        {
            tileset = $"{_options.Account}.{tileset}",
            credentials.url,
            name,
        };

        string jsonBody = JsonSerializer.Serialize(requestBody);
        using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"uploads/v1/{_options.Account}?access_token={_options.ApiKey}", content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Mapbox API call failed with status code {StatusCode}", response.StatusCode);

            throw new HttpRequestException("Mapbox API call failed");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<MapboxUploadResponse>(jsonResponse);

        // return responseObject;
    }

    /// <summary>
    ///     Check if the service is available.
    /// </summary>
    public async Task HealthCheck()
    {
        var response = await httpClient.GetAsync($"datasets/v1/{_options.Account}?access_token={_options.ApiKey}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("Mapbox API call failed");
        }
    }

    /// <summary>
    ///     Free managed resources.
    /// </summary>
    public void Dispose() => httpClient.Dispose();
}
