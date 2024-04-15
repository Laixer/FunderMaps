using System.Collections;

namespace FunderMaps.Core.Exceptions;

/// <summary>
///     Default exception for the core assembly.
/// </summary>
/// <remarks>
///     All exception in this assembly ought to inherit from this
///     exception
/// </remarks>
public abstract class FunderMapsCoreException : Exception, IEnumerable<Exception>
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public FunderMapsCoreException()
        : base("Application was unable to process the request.")
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public FunderMapsCoreException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public FunderMapsCoreException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    ///     IEnumerable interface implementation
    /// </summary>
    public IEnumerator<Exception> GetEnumerator()
    {
        Exception _exception = this;
        while (_exception is not null)
        {
            yield return _exception;
            if (_exception.InnerException is not null)
            {
                _exception = _exception.InnerException;
            }
        }
    }

    /// <summary>
    ///     IEnumerable interface implementation
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
