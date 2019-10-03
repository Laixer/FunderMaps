using System;

namespace FunderMaps.Core.Entities
{
    // FUTURE: can be removed.
    /// <summary>
    /// Report norm.
    /// </summary>
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
