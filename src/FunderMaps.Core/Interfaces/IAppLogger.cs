namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    /// Throughout the application core this logger should be used.
    /// </summary>
    public interface IAppLogger
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
    }
}
