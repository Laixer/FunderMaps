namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Geospatial abstraction service.
/// </summary>
public interface IGDALService
{
    /// <summary>
    ///     Convert geospatial file from one format to another.
    /// </summary>
    /// <param name="input">Input file.</param>
    /// <param name="output">Output file.</param>
    /// <param name="layer">Layer name.</param>
    void Convert(string input, string output, string? layer = null);
}
