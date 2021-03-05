using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Entity could not be found.
    /// </summary>
    public class EntityNotFoundException : FunderMapsCoreException
    {
        /// <summary>
        ///     Exception title
        /// </summary>
        public override string Title => "Requested entity not found.";

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public EntityNotFoundException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
