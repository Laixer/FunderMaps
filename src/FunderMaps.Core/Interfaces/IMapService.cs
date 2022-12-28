namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Service to manage and publish maps.
    /// </summary>
    public interface IMapService
    {
        /// <summary>
        ///     Delete dataset from mapping service.
        /// </summary>
        /// <param name="datasetName">The dataset name.</param>
        Task<bool> DeleteDatasetAsync(string datasetName);

        /// <summary>
        ///     Upload dataset to mapping service.
        /// </summary>
        /// <param name="datasetName">The dataset name.</param>
        /// <param name="filePath">Path to dataset on disk.</param>
        Task UploadDatasetAsync(string datasetName, string filePath);

        /// <summary>
        ///     Upload dataset feature to mapping service.
        /// </summary>
        /// <param name="datasetName">The dataset name.</param>
        /// <param name="featureId">Feature identifier.</param>
        /// <param name="geoJsonObject">GeoJSON object.</param>
        Task UploadDatasetFeatureAsync(string datasetName, string featureId, object geoJsonObject);

        Task<bool> UploadDatasetToTilesetAsync(string datasetName, string tileset);

        Task<bool> UploadStatusAsync(string uploadId);

        /// <summary>
        ///     Publish dataset as map.
        /// </summary>
        /// <param name="datasetName">The dataset name.</param>
        Task<bool> PublishAsync(string datasetName);
    }
}
