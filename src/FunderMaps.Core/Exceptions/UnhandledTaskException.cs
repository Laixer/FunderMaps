namespace FunderMaps.Core.Exceptions;

/// <summary>
///     Task was not executed.
/// </summary>
public class UnhandledTaskException : FunderMapsCoreException
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public UnhandledTaskException()
        : base("Application was unable to process the request.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public UnhandledTaskException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public UnhandledTaskException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
