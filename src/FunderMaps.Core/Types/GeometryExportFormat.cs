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
        Mvt,

        /// <summary>
        ///     Geopackage.
        /// </summary>
        Gpkg,

        /// <summary>
        ///     Shapefile.
        /// </summary>
        Shp,

        /// <summary>
        ///     GeoJSON.
        /// </summary>
        GeoJSON,
    }
}
