namespace FunderMaps.Core.Exceptions;

/// <summary>
///     Exception indicating an unhandled database error.
/// </summary>
public sealed class DatabaseException : FunderMapsCoreException
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public DatabaseException()
        : base("Unhandled database error.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    /// <param name="message"><see cref="Exception.Message"/></param>
    public DatabaseException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    /// <param name="message"><see cref="Exception.Message"/></param>
    /// <param name="innerException"><see cref="Exception.InnerException"/></param>
    public DatabaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
