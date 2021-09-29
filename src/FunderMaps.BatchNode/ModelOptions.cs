namespace FunderMaps.BatchNode
{
    /// <summary>
    ///     Model options.
    /// </summary>
    public sealed record ModelOptions
    {
        /// <summary>
        ///     Configuration section key.
        /// </summary>
        public const string Section = "Model";

        /// <summary>
        ///     Batch service interval in hours.
        /// </summary>
        public int Interval { get; set; }
    }
}
