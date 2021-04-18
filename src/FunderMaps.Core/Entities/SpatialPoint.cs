namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Entity representing a geospatial point.
    /// </summary>
    public sealed class SpatialPoint
    {
        /// <summary>
        ///     Center Y.
        /// </summary>
        public double? CenterX { get; set; }

        /// <summary>
        ///     Center X.
        /// </summary>
        public double? CenterY { get; set; }
    }
}
