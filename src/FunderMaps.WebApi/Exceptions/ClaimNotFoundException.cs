using System;

namespace FunderMaps.Exceptions
{
    /// <summary>
    /// Claim was not found exception.
    /// </summary>
    public class ClaimNotFoundException : Exception
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="claimType">Type of claim that was not found.</param>
        public ClaimNotFoundException(string claimType)
            : base($"Claim with type {claimType} was not found.")
        { }

        /// <summary>
        /// Create new instance.
        /// </summary>
        public ClaimNotFoundException()
        { }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Internal exception.</param>
        public ClaimNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
