using System;

namespace TdmClient.Exceptions
{
    /// <summary>
    /// Request unauthorized exception.
    /// </summary>
    public class UnauthorizedRequestException : Exception
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public UnauthorizedRequestException()
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public UnauthorizedRequestException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public UnauthorizedRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
