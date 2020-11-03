using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;

namespace FunderMaps.Console.Types
{
    /// <summary>
    ///     Context for executing a bundle build task.
    /// </summary>
    public class BundleBuildingContext
    {
        /// <summary>
        ///     The id of the bundle to process.
        /// </summary>
        public Guid BundleId { get; set; }

        /// <summary>
        ///     Contains all formats we wish to export.
        /// </summary>
        public IEnumerable<GeometryExportFormat> Formats { get; set; }
    }
}
