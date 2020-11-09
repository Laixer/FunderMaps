using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Exception indicating our task queue is full.
    /// </summary>
    public sealed class QueueFullException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueFullException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueFullException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueFullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public QueueFullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
