namespace FunderMaps.Core.Exceptions;

/// <summary>
///     Indicates something went wrong with our storage.
/// </summary>
public class StorageException : FunderMapsCoreException
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public StorageException()
        : base("Application was unable to process the request.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public StorageException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public StorageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
