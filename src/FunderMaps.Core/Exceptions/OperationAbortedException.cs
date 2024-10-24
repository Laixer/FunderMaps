namespace FunderMaps.Core.Exceptions;

/// <summary>
///     Exception indicating a product request was invalid.
/// </summary>
public sealed class OperationAbortedException : FunderMapsCoreException
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public OperationAbortedException()
        : base("Operation was aborted by client.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    /// <param name="message"><see cref="Exception.Message"/></param>
    public OperationAbortedException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    /// <param name="message"><see cref="Exception.Message"/></param>
    /// <param name="innerException"><see cref="Exception.InnerException"/></param>
    public OperationAbortedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
