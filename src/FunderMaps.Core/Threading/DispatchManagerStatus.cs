namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Dispatch manager task status.
    /// </summary>
    public record DispatchManagerStatus
    {
        /// <summary>
        ///     Number of jobs succeeded.
        /// </summary>
        public int JobsSucceeded { get; set; }

        /// <summary>
        ///     Number of jobs failed.
        /// </summary>
        public int JobsFailed { get; set; }
    }
}
