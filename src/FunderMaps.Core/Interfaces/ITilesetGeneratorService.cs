namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Tileset generator service.
/// </summary>
public interface ITilesetGeneratorService
{
    /// <summary>
    ///     Generate vector tiles.
    /// </summary>
    /// <param name="input">Input file.</param>
    /// <param name="output">Output file.</param>
    /// <param name="layer">Layer name.</param>
    /// <param name="maxZoomLevel">Maximum zoom level.</param>
    /// <param name="minZoomLevel">Minimum zoom level.</param>
    void Generate(string input, string output, string? layer = null, int maxZoomLevel = 15, int minZoomLevel = 10);
}
