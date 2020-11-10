using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Generic protocol exception.
    /// </summary>
    public class ProtocolException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProtocolException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProtocolException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProtocolException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
