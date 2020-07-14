using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Repository exception.
    /// </summary>
    /// <remarks>
    ///     All repository related exceptions should take this exception 
    ///     as its base. Internal core operations test for this abstract
    ///     exception.
    ///     <para>
    ///         This exception can be instantiated directly when no
    ///         specifc derived exception fits the case.
    ///     </para>
    /// </remarks>
    public class RepositoryException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RepositoryException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RepositoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        protected RepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
