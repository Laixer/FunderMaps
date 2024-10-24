namespace FunderMaps.Core.Exceptions;

/// <summary>
///     A local or remote service is unavailable.
/// </summary>
public class ServiceUnavailableException : FunderMapsCoreException
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ServiceUnavailableException()
        : base("Internal service was unable to process the request.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ServiceUnavailableException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ServiceUnavailableException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
