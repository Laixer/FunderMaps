using System;

namespace FunderMaps.Core.Identity
{
    /// <summary>
    ///     Tenant identity.
    /// </summary>
    public interface ITenant
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        Guid Id { get; set; }
    }
}
