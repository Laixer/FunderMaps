using System;
using System.Runtime.Serialization;

namespace FunderMaps.Console.Exceptions
{
    /// <summary>
    ///     Indicates a type of task is not supported.
    /// </summary>
    public sealed class TaskTypeNotSupportedException : ConsoleCoreException
    {
        public TaskTypeNotSupportedException()
        {
        }

        public TaskTypeNotSupportedException(string message) : base(message)
        {
        }

        public TaskTypeNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TaskTypeNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
