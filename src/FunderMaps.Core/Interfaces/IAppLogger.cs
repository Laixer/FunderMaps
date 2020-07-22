namespace FunderMaps.Core.Interfaces
{
    // TODO: Replace by logger abstractions

    /// <summary>
    ///     Core application logger.
    /// </summary>
    /// <remarks>
    ///     Throughout the application core this logger should be used.
    /// </remarks>
    public interface IAppLogger
    {
        /// <summary>
        ///     Log level 'information'.
        /// </summary>
        void LogInformation(string message, params object[] args);
        
        /// <summary>
        ///     Log level 'warning'.
        /// </summary>
        void LogWarning(string message, params object[] args);
    }
}
