namespace FunderMaps.Core.Exceptions;

/// <summary>
///     Entity cannot be modified.
/// </summary>
public class EntityReadOnlyException : FunderMapsCoreException
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public EntityReadOnlyException()
        : base("Requested entity is immutable.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public EntityReadOnlyException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public EntityReadOnlyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
