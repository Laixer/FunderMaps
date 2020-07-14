using System;
#if AKAS
namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Geometric polygon.
    /// </summary>
    [Obsolete]
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
#endif