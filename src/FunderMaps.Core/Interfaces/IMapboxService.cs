namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Mapbox service.
/// </summary>
public interface IMapboxService : IServiceHealthCheck
{
    /// <summary>
    ///     Upload file to Mapbox.
    /// </summary>
    /// <param name="name">Name of the file.</param>
    /// <param name="tileset">Tileset name.</param>
    /// <param name="filePath">Path to file.</param>
    Task UploadAsync(string name, string tileset, string filePath);
}
