namespace FunderMaps.Tdm
{
    /// <summary>
    /// Shared sync state.
    /// </summary>
    internal class SharedSyncState
    {
        /// <summary>
        /// Syncpoint for OG 'Wonen'.
        /// </summary>
        public uint SyncpointWonen { get; set; }

        /// <summary>
        /// Syncpoint for OG 'Business'.
        /// </summary>
        public uint SyncpointBusiness { get; set; }

        /// <summary>
        /// Syncpoint for OG 'Alv'.
        /// </summary>
        public uint SyncpointAlv { get; set; }

        /// <summary>
        /// Syncpoint for 'Media'.
        /// </summary>
        public uint SyncpointMedia { get; set; }
    }
}