using FunderMaps.Core.Interfaces;

namespace FunderMaps.Core.Services;

/// <summary>
///     Dummy map service.
/// </summary>
internal class NullMapService : IMapService
{
    /// <summary>
    ///     Delete dataset from mapping service.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    public Task<bool> DeleteDatasetAsync(string datasetName)
        => Task.FromResult(true);

    /// <summary>
    ///     Upload dataset to mapping service.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    /// <param name="filePath">Path to dataset on disk.</param>
    public Task UploadDatasetAsync(string datasetName, string filePath)
        => Task.CompletedTask;

    /// <summary>
    ///     Upload dataset feature to mapping service.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    /// <param name="featureId">Feature identifier.</param>
    /// <param name="geoJsonObject">GeoJSON object.</param>
    public Task UploadDatasetFeatureAsync(string datasetName, string featureId, object geoJsonObject)
        => Task.CompletedTask;

    public Task<bool> UploadDatasetToTilesetAsync(string datasetName, string tileset) => Task.FromResult(true);

    /// <summary>
    ///     Publish dataset as map.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    public Task<bool> PublishAsync(string datasetName)
        => Task.FromResult(true);

    public Task<bool> UploadStatusAsync(string uploadId) => Task.FromResult(true);

    /// <summary>
    ///     Test the service backend.
    /// </summary>
    public Task HealthCheck() => Task.CompletedTask;


}
