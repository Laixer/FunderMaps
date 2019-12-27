using System;

namespace TdmClient.Exceptions
{
    /// <summary>
    /// Invalid request exception.
    /// </summary>
    public class InvalidRequestParameterException : Exception
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public InvalidRequestParameterException()
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public InvalidRequestParameterException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidRequestParameterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
