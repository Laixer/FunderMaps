using FunderMaps.Core.Services;

namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Mapbox service.
/// </summary>
public interface IMapboxService
{
    /// <summary>
    ///     Upload file to Mapbox.
    /// </summary>
    /// <param name="name">Name of the file.</param>
    /// <param name="tileset">Tileset name.</param>
    /// <param name="filePath">Path to file.</param>
    Task<MapboxUploadResponse> UploadAsync(string name, string tileset, string filePath);
}
