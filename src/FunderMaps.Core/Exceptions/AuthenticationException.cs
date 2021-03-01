using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Authentication faillure.
    /// </summary>
    public class AuthenticationException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthenticationException()
            : base("Login attempt failed with provided credentials.")
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthenticationException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
