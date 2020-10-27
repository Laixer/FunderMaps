using FunderMaps.Core.Exceptions;
using System;
using System.Runtime.Serialization;

namespace FunderMaps.Console.Exceptions
{
    /// <summary>
    ///     Base exception for all console related errors.
    /// </summary>
    public abstract class ConsoleCoreException : FunderMapsCoreException
    {
        public ConsoleCoreException()
        {
        }

        public ConsoleCoreException(string message) : base(message)
        {
        }

        public ConsoleCoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConsoleCoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
