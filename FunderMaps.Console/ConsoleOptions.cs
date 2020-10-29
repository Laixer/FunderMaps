namespace FunderMaps.Console
{
    /// <summary>
    ///     Wrapper for all our console options.
    /// </summary>
    public sealed class ConsoleOptions
    {
        /// <summary>
        ///     Defines the maximum items in our queue.
        /// </summary>
        /// <remarks>
        ///     Defaults to 128.
        /// </remarks>
        public uint MaxQueueSize { get; set; } = 128;

        // TODO We already have these properties in the connection string, might be a bit too double?
        /// <summary>
        ///     The database host address.
        /// </summary>
        public string DatabaseHost { get; set; }

        /// <summary>
        ///     The database username.
        /// </summary>
        public string DatabaseUser { get; set; }

        /// <summary>
        ///     The database user password.
        /// </summary>
        public string DatabasePassword { get; set; }

        /// <summary>
        ///     The name of the database to extract from.
        /// </summary>
        public string DatabaseName { get; set; }

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
