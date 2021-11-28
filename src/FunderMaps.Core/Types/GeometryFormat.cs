namespace FunderMaps.Core.Types;

/// <summary>
///     Represents a type of export format.
/// </summary>
public enum GeometryFormat
{
    /// <summary>
    ///     Mapbox vector tiles.
    /// </summary>
    MapboxVectorTiles = 0,

    /// <summary>
    ///     GeoPackage.
    /// </summary>
    GeoPackage = 1,

    /// <summary>
    ///     ESRI Shapefile.
    /// </summary>
    ESRIShapefile = 2,

    /// <summary>
    ///     GeoJSON.
    /// </summary>
    GeoJSON = 3,

    /// <summary>
    ///     GeoJSON Sequential.
    /// </summary>
    GeoJSONSeq = 4,
}
