namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Entity representing a geospatial box.
    /// </summary>
    public sealed class SpatialBox
    {
        /// <summary>
        ///     Area X min.
        /// </summary>
        public double? XMin { get; set; }

        /// <summary>
        ///     Area Y min.
        /// </summary>
        public double? YMin { get; set; }

        /// <summary>
        ///     Area X max.
        /// </summary>
        public double? XMax { get; set; }

        /// <summary>
        ///     Area Y max.
        /// </summary>
        public double? YMax { get; set; }
    }
}
