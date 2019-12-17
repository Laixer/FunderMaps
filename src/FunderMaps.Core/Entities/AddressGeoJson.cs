using System.Collections.Generic;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Geometric polygon.
    /// </summary>
    public class AddressGeoJson : Address2
    {
        /// <summary>
        /// GeoJson as string.
        /// </summary>
        public string GeoJson { get; set; }

        /// <summary>
        /// RGB color.
        /// </summary>
        public object Color { get; set; }
    }
}
