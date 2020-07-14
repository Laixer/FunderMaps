using FunderMaps.Core.Exceptions;
using System;

namespace FunderMaps.Data.Exceptions
{
    /// <summary>
    ///     This exception is thrown when an empty resultset is returned.
    /// </summary>
    public class NullRowException : RepositoryException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NullRowException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NullRowException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NullRowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
