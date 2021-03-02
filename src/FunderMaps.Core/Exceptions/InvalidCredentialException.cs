using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Invalid credentials provided.
    /// </summary>
    public class InvalidCredentialException : FunderMapsCoreException
    {
        /// <summary>
        ///     Exception title
        /// </summary>
        public new const string Title = "Action failed with provided credentials.";


        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InvalidCredentialException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InvalidCredentialException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InvalidCredentialException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
