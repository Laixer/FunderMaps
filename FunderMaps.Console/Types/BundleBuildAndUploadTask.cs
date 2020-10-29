using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;

namespace FunderMaps.Console.Types
{
    /// <summary>
    ///     Task representing building a bundle and uploading
    ///     the results to our blob storage.
    /// </summary>
    /// <remarks>
    ///     - Get current bundle version
    ///     - Build bundle in temp
    ///     - Upload bundle
    ///     - Remove all from temp
    /// </remarks>
    public class BundleBuildAndUploadTask : BackgroundTask
    {
        /// <summary>
        ///     The bundle id.
        /// </summary>
        public Guid BundleId { get; set; }

        /// <summary>
        ///     All formats to export the bundle to.
        /// </summary>
        public IEnumerable<GeometryExportFormat> Formats { get; set; }
    }
}
