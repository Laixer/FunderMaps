using System;

namespace TdmClient.Exceptions
{
    /// <summary>
    /// Remote service unavailable exception.
    /// </summary>
    public class RemoteServiceUnavailableException : Exception
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public RemoteServiceUnavailableException()
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public RemoteServiceUnavailableException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public RemoteServiceUnavailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
