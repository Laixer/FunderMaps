using FunderMaps.Core.Exceptions;
using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Base exception for all console related errors.
    /// </summary>
    public abstract class BackgroundWorkBaseException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BackgroundWorkBaseException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BackgroundWorkBaseException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BackgroundWorkBaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        protected BackgroundWorkBaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
