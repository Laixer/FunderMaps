namespace FunderMaps.Core.Exceptions;

/// <summary>
///     Exception indicating queue hit item limit.
/// </summary>
public sealed class QueueOverflowException : FunderMapsCoreException
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public QueueOverflowException()
        : base("Application was unable to process the request.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public QueueOverflowException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public QueueOverflowException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
