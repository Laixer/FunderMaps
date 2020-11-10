namespace FunderMaps.Core.Types.MapLayer
{
    /// <summary>
    ///     Bundle status enum
    /// </summary>
    public enum BundleStatus
    {
        /// <summary>
        ///     This bundle has just been created.
        /// </summary>
        Created,

        /// <summary>
        ///     The bundle is being processed by some external entity.
        ///     The current version is still accurate.
        /// </summary>
        Processing,

        /// <summary>
        ///     The bundle is up to date and the current version is accurate.
        /// </summary>
        UpToDate,

        /// <summary>
        ///     The bundle has been deleted.
        /// </summary>
        Deleted,
    }
}
