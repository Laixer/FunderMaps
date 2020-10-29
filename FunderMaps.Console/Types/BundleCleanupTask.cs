using System;

namespace FunderMaps.Console.Types
{
    // FUTURE Implement service for this.
    /// <summary>
    ///     Task representing bundle cleanup in the cloud.
    /// </summary>
    public class BundleCleanupTask : BackgroundTask
    {
        /// <summary>
        ///     The bundle id.
        /// </summary>
        /// <remarks>
        ///     - Get current bundle version
        ///     - Remove all older versions from the blob storage
        /// </remarks>
        public Guid BundleId { get; set; }
    }
}
