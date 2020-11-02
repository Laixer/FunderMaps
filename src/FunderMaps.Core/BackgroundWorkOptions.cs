namespace FunderMaps.Core
{
    /// <summary>
    ///     Wrapper for all our console options.
    /// </summary>
    public sealed class BackgroundWorkOptions
    {
        /// <summary>
        ///     Defines the maximum items in our queue.
        /// </summary>
        public uint MaxQueueSize { get; set; } = 1024;

        /// <summary>
        ///     The amount of simultaneous running background workers
        ///     for synchronous work.
        /// </summary>
        public uint MaxWorkers { get; set; }
    }
}
