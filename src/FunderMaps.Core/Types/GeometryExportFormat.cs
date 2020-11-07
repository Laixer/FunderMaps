namespace FunderMaps.Core.Types
{
    /// <summary>
    ///     Represents a type of export format.
    /// </summary>
    public enum GeometryExportFormat
    {
        /// <summary>
        ///     Mapbox vector tiles.
        /// </summary>
        MapboxVectorTiles,

        /// <summary>
        ///     GeoPackage.
        /// </summary>
        GeoPackage,

        /// <summary>
        ///     ESRI Shapefile.
        /// </summary>
        ESRIShapefile,

        /// <summary>
        ///     GeoJSON.
        /// </summary>
        GeoJSON,
    }
}
