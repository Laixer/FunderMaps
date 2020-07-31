using GeoJSON.Net.Geometry;

namespace FunderMaps.Webservice.Types
{
    /// <summary>
    /// Represents an area.
    /// </summary>
    public abstract class Region
    {
        /// <summary>
        /// Represents the name of this region.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Multipolygon representation of this region.
        /// </summary>
        public MultiPolygon Geometry { get; set; }
    }
}
