using System.Threading.Tasks;

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
        Task<bool> UploadDatasetAsync(string datasetName, string filePath);

        /// <summary>
        ///     Publish dataset as map.
        /// </summary>
        /// <param name="datasetName">The dataset name.</param>
        Task<bool> PublishAsync(string datasetName);
    }
}
