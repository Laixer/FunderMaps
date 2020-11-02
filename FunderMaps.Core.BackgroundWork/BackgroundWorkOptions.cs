namespace FunderMaps.Core.BackgroundWork
{
    /// <summary>
    ///     Wrapper for all our console options.
    /// </summary>
    public sealed class BackgroundWorkOptions
    {
        /// <summary>
        ///     Defines the maximum items in our queue.
        /// </summary>
        /// <remarks>
        ///     Defaults to 128.
        /// </remarks>
        public uint MaxQueueSize { get; set; } = 128;

        /// <summary>
        ///     The amount of simultaneous running background workers
        ///     for synchronous work.
        /// </summary>
        public uint MaxWorkers { get; set; }
    }
}
