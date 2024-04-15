namespace FunderMaps.Core.Exceptions;

/// <summary>
///     Referenced entity could not be found.
/// </summary>
public class ReferenceNotFoundException : FunderMapsCoreException
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ReferenceNotFoundException()
        : base("Referenced entity not found.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ReferenceNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ReferenceNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
