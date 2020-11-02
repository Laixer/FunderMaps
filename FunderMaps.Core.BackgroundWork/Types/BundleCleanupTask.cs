using System;

namespace FunderMaps.Core.BackgroundWork.Types
{
    // FUTURE Implement service for this.
    /// <summary>
    ///     Task representing bundle cleanup in the cloud.
    /// </summary>
    public class BundleCleanupTask : BackgroundTask
    {
        /// <summary>
        ///     This task should run synchronously.
        /// </summary>
        public override bool RunSynchronously => false;

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
