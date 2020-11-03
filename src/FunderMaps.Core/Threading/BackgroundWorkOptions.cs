namespace FunderMaps.Core.Threading
{
    /// <summary>
    ///     Wrapper for all our console options.
    /// </summary>
    public sealed class BackgroundWorkOptions
    {
        /// <summary>
        ///     Defines the maximum items in our queue.
        /// </summary>
        public int MaxQueueSize { get; set; } = 1024;

        /// <summary>
        ///     The amount of simultaneous running background workers
        ///     for synchronous work.
        /// </summary>
        public int MaxWorkers { get; set; }
    }
}
