﻿using System;
using System.Runtime.Serialization;

namespace FunderMaps.Console.Exceptions
{
    /// <summary>
    ///     Exception indicating our task queue is full.
    /// </summary>
    public sealed class QueueFullException : ConsoleCoreException
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
