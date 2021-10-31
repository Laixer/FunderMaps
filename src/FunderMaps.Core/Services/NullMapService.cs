using FunderMaps.Core.Interfaces;
using System.Threading.Tasks;

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
    public Task<bool> UploadDatasetAsync(string datasetName, string filePath)
        => Task.FromResult(true);

    /// <summary>
    ///     Publish dataset as map.
    /// </summary>
    /// <param name="datasetName">The dataset name.</param>
    public Task<bool> PublishAsync(string datasetName)
        => Task.FromResult(true);

    /// <summary>
    ///     Test the service backend.
    /// </summary>
    public Task HealthCheck() => Task.CompletedTask;
}
