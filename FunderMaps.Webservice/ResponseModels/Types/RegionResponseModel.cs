using GeoJSON.Net.Geometry;

namespace FunderMaps.Webservice.ResponseModels.Types
{
    /// <summary>
    /// Response model representing a region.
    /// </summary>
    public sealed class RegionResponseModel
    {
        /// <summary>
        /// Represents the name of this region.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the type of region.
        /// </summary>
        public string RegionType { get; set; }

        /// <summary>
        /// Multipolygon representation of this region.
        /// </summary>
        public MultiPolygon GeoData { get; set; }
    }
}
