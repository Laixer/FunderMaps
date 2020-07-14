using System;
#if KAAS
namespace FunderMaps.Core.Entities
{
    // FUTURE: can be removed.
    /// <summary>
    /// Report norm.
    /// </summary>
    [Obsolete]
    public class Contractor
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Contractor name.
        /// </summary>
        public string Name { get; set; }
    }
}
#endif