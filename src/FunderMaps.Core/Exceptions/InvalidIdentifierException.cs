using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Invalid identifier provided.
    /// </summary>
    public class InvalidIdentifierException : FunderMapsCoreException
    {
        /// <summary>
        ///     Exception title
        /// </summary>
        public override string Title => "Action failed with provided identifier.";

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InvalidIdentifierException()
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
