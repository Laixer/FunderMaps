using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.BackgroundWork.Exceptions
{
    /// <summary>
    ///     Exception indicating our task queue is full.
    /// </summary>
    public sealed class QueueFullException : BackgroundWorkBaseException
    {
        public QueueFullException()
        {
        }

        public QueueFullException(string message) : base(message)
        {
        }

        public QueueFullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public QueueFullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
