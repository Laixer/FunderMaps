using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;

namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Context for executing a bundle build task.
    /// </summary>
    public record BundleBuildingContext
    {
        /// <summary>
        ///     The id of the bundle to process.
        /// </summary>
        public Guid BundleId { get; init; }

        /// <summary>
        ///     Contains all formats we wish to export.
        /// </summary>
        public IEnumerable<GeometryFormat> Formats { get; set; }
    }
}
