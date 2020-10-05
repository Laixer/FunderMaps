using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Indicates the situation where a user tries to assign himself
    ///     as reviewer on an inquiry that he or she has created.
    /// </summary>
    public sealed class SelfReviewException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public SelfReviewException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public SelfReviewException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="innerException">The inner <see cref="Exception"/>.</param>
        public SelfReviewException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
