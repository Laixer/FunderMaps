using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FunderMaps.Infrastructure.Storage
{
    /// <summary>
    ///     Mapbox implementation of <see cref="IMapService"/>.
    /// </summary>
    internal class MapboxService : IMapService
    {
        private const string mapboxBaseUrl = "https://api.mapbox.com";
        private const int timeoutInMinutes = 30;

        private readonly MapboxOptions _options;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public MapboxService(IOptions<MapboxOptions> options)
            => _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        /// <summary>
        ///     Create a HTTP client.
        /// </summary>
        protected HttpClient CreateClient()
        {
            HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);

            return client;
        }

        /// <summary>
        ///     Delete dataset from mapping service.
        /// </summary>
        /// <param name="datasetName">The dataset name.</param>
        public async Task<bool> DeleteDatasetAsync(string datasetName)
        {
            using HttpClient client = CreateClient();

            HttpResponseMessage result = await client.DeleteAsync($"{mapboxBaseUrl}/tilesets/v1/sources/{_options.Account}/{datasetName}?access_token={_options.AccessToken}");

            return result.IsSuccessStatusCode;
        }

        /// <summary>
        ///     Upload dataset to mapping service.
        /// </summary>
        /// <param name="datasetName">The dataset name.</param>
        /// <param name="filePath">Path to dataset on disk.</param>
        public async Task<bool> UploadDatasetAsync(string datasetName, string filePath)
        {
            using HttpClient client = CreateClient();

            using MultipartFormDataContent content = new($"Upload----{DateTime.Now}");

            using StreamContent fileContent = new(File.OpenRead(filePath));
            content.Add(fileContent, "file", Path.GetFileName(filePath));
            HttpResponseMessage result = await client.PostAsync($"{mapboxBaseUrl}/tilesets/v1/sources/{_options.Account}/{datasetName}?access_token={_options.AccessToken}", content);

            return result.IsSuccessStatusCode;
        }

        /// <summary>
        ///     Publish dataset as map.
        /// </summary>
        /// <param name="datasetName">The dataset name.</param>
        public async Task<bool> PublishAsync(string datasetName)
        {
            using HttpClient client = CreateClient();

            HttpResponseMessage result = await client.PostAsync($"{mapboxBaseUrl}/tilesets/v1/{_options.Account}.{datasetName}/publish?access_token={_options.AccessToken}", null);

            return result.IsSuccessStatusCode;
        }

        /// <summary>
        ///     Test the Mapbox service backend.
        /// </summary>
        public async Task HealthCheck()
        {
            using HttpClient client = CreateClient();

            await client.GetAsync($"{mapboxBaseUrl}/tilesets/v1/{_options.Account}?access_token={_options.AccessToken}");
        }
    }
}
