using FunderMaps.Core.Exceptions;
using System;

namespace FunderMaps.Data.Exceptions
{
    /// <summary>
    ///     This exception is thrown when no rows were affected.
    /// </summary>
    public class NullAffectedException : RepositoryException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NullAffectedException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NullAffectedException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public NullAffectedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
