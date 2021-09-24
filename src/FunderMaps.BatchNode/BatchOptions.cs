namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Batch node options.
    /// </summary>
    public sealed record BatchOptions
    {
        /// <summary>
        ///     Configuration section key.
        /// </summary>
        public const string Section = "Batch";

        /// <summary>
        ///     Batch service interval in hours.
        /// </summary>
        public int Interval { get; set; }
    }
}
