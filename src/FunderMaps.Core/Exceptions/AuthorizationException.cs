using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Authorization faillure.
    /// </summary>
    public class AuthorizationException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthorizationException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthorizationException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
