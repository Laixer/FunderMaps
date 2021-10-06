namespace FunderMaps.Data.Providers
{
    /// <summary>
    ///     Database provider options.
    /// </summary>
    internal class DbProviderOptions
    {
        /// <summary>
        ///     Database connection name.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        ///     The client application name.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        ///     Minimum number of connection to keep in the pool.
        /// </summary>
        public int MinPoolSize { get; set; }

        /// <summary>
        ///     Maximum number of connection to keep in the pool.
        /// </summary>
        public int MaxPoolSize { get; set; } = 25;

        /// <summary>
        ///     The time in seconds to wait for a connection to open.
        /// </summary>
        public int ConnectionTimeout { get; set; } = 10;

        /// <summary>
        ///     The time in seconds to wait for the command to execute.
        /// </summary>
        public int CommandTimeout { get; set; } = 10;
    }
}
