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
    }
}
