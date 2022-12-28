using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace FunderMaps.Infrastructure.Storage;

/// <summary>
///     Mapbox implementation of <see cref="IMapService"/>.
/// </summary>
internal class MapboxService : IMapService
{
    private const string mapboxBaseUrl = "https://api.mapbox.com";
    private const int timeoutInMinutes = 30;

    private readonly MapboxOptions _options;

    private static readonly HttpClient _httpClient = new();

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public MapboxService(IOptions<MapboxOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        _httpClient.BaseAddress = new Uri(mapboxBaseUrl);
        _httpClient.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    ///     Delete dataset from mapping service.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    public async Task<bool> DeleteDatasetAsync(string datasetName)
    {
        HttpResponseMessage result = await _httpClient.DeleteAsync($"tilesets/v1/sources/{_options.Account}/{datasetName}?access_token={_options.AccessToken}");

        return result.IsSuccessStatusCode;
    }

    /// <summary>
    ///     Upload dataset to mapping service.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    /// <param name="filePath">Path to dataset on disk.</param>
    public async Task UploadDatasetAsync(string datasetName, string filePath)
    {
        using MultipartFormDataContent content = new($"Upload----{DateTime.Now}");

        using StreamContent fileContent = new(File.OpenRead(filePath));
        content.Add(fileContent, "file", Path.GetFileName(filePath));

        HttpResponseMessage response = await _httpClient.PostAsync($"tilesets/v1/sources/{_options.Account}/{datasetName}?access_token={_options.AccessToken}", content);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    ///     Upload dataset feature to mapping service.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    /// <param name="featureId">Feature identifier.</param>
    /// <param name="geoJsonObject">GeoJSON object.</param>
    public async Task UploadDatasetFeatureAsync(string datasetName, string featureId, object geoJsonObject)
    {
        HttpRequestMessage request = new(HttpMethod.Put, $"datasets/v1/{_options.Account}/{datasetName}/features/{featureId}?access_token={_options.AccessToken}");

        using StringContent content = new(geoJsonObject.ToString(), System.Text.Encoding.UTF8, "application/json");

        request.Content = content;

        using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    ///     Upload dataset feature to mapping service.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    /// <param name="featureId">Feature identifier.</param>
    /// <param name="geoJsonObject">GeoJSON object.</param>
    public async Task<bool> UploadDatasetToTilesetAsync(string datasetName, string tileset)
    {
        using StringContent content = new(System.Text.Json.JsonSerializer.Serialize(new
        {
            url = $"mapbox://datasets/laixer/{datasetName}",
            tileset = $"laixer.{tileset}",
            name = tileset,
        }), System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage result = await _httpClient.PostAsync($"uploads/v1/{_options.Account}?access_token={_options.AccessToken}", content);

        return result.IsSuccessStatusCode;
    }

    public record UploadStatus
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
        public string Error { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Tileset { get; set; }
        public string Owner { get; set; }
        public int Progress { get; set; }
    }

    public async Task<bool> UploadStatusAsync(string uploadId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"uploads/v1/{_options.Account}/{uploadId}?access_token={_options.AccessToken}");

        var contentStream = await response.Content.ReadAsStreamAsync();

        var status = await System.Text.Json.JsonSerializer.DeserializeAsync<UploadStatus>(contentStream);
        return status.Complete;
    }

    /// <summary>
    ///     Publish dataset as map.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    public async Task<bool> PublishAsync(string datasetName)
    {
        HttpResponseMessage result = await _httpClient.PostAsync($"tilesets/v1/{_options.Account}.{datasetName}/publish?access_token={_options.AccessToken}", null);

        return result.IsSuccessStatusCode;
    }

    /// <summary>
    ///     Test the Mapbox service backend.
    /// </summary>
    public async Task HealthCheck()
    {
        await _httpClient.GetAsync($"tilesets/v1/{_options.Account}?access_token={_options.AccessToken}");
    }
}
