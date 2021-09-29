namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Map bundle options.
    /// </summary>
    public sealed record MapBundleOptions
    {
        /// <summary>
        ///     Configuration section key.
        /// </summary>
        public const string Section = "MapBundle";

        /// <summary>
        ///     Batch service interval in hours.
        /// </summary>
        public int Interval { get; set; }
    }
}
