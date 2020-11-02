namespace FunderMaps.Console
{
    /// <summary>
    ///     Configuration for our bundle builder.
    /// </summary>
    public sealed class BundleBuildingOptions
    {
        /// <summary>
        ///  Name of our connection string.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        ///     Minimum mvt tile zoom.
        /// </summary>
        public uint MvtMinZoom { get; set; }

        /// <summary>
        ///     Maximum mvt tile zoom.
        /// </summary>
        public uint MvtMaxZoom { get; set; }
    }
}
