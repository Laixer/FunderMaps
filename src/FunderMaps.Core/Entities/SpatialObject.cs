using System;

namespace FunderMaps.Core.Entities.Fis
{
    /// <summary>
    /// Spatial object.
    /// </summary>
    public class SpatialObject
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// BAG identifier.
        /// </summary>
        public long? Bag { get; set; }
    }
}
