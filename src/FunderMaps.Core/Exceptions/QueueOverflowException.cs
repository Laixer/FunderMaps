using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Exception indicating queue hit item limit.
    /// </summary>
    public sealed class QueueOverflowException : FunderMapsCoreException
    {
        /// <summary>
        ///     Exception title
        /// </summary>
        public override string Title => "Application was unable to process the request.";

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueOverflowException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueOverflowException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueOverflowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueOverflowException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
