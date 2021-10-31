using System;

namespace TdmClient.Exceptions;

/// <summary>
/// Resource forbidden exception.
/// </summary>
public class ResourceForbiddenException : Exception
{
    /// <summary>
    /// Create new instance.
    /// </summary>
    public ResourceForbiddenException()
    {
    }

    /// <summary>
    /// Create new instance.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public ResourceForbiddenException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Create new instance.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Inner exception.</param>
    public ResourceForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
