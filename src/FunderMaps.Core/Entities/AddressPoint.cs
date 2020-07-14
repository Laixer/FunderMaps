using System;
#if KAAS
namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Geometric point.
    /// </summary>
    [Obsolete]
    public class AddressPoint : Address
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z coordinate.
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// RGB color.
        /// </summary>
        public object Color { get; set; }
    }
}
#endif