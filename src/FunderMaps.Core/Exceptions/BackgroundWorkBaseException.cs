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
        public BackgroundWorkBaseException()
        {
        }

        public BackgroundWorkBaseException(string message) : base(message)
        {
        }

        public BackgroundWorkBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BackgroundWorkBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
