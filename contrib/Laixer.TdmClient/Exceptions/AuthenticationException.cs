using System;

namespace TdmClient.Exceptions
{
    /// <summary>
    /// Authentication exception.
    /// </summary>
    public class AuthenticationException : Exception
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public AuthenticationException()
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public AuthenticationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
