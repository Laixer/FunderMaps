using System;

namespace TdmClient.Exceptions;

/// <summary>
/// Resource not found exception.
/// </summary>
public class ResourceNotFoundException : Exception
{
    /// <summary>
    /// Create new instance.
    /// </summary>
    public ResourceNotFoundException()
    {
    }

    /// <summary>
    /// Create new instance.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public ResourceNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Create new instance.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Inner exception.</param>
    public ResourceNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
