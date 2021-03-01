using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Invalid identifier provided.
    /// </summary>
    public class InvalidIdentifierException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InvalidIdentifierException()
            : base("Action failed with provided identifier.")
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InvalidIdentifierException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InvalidIdentifierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
