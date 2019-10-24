using Microsoft.AspNetCore.Identity;
using System;

namespace FunderMaps.Models.Identity
{
    /// <summary>
    /// Application role.
    /// </summary>
    public class FunderMapsRole : IdentityRole<Guid>
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public FunderMapsRole() { }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="roleName">Application role.</param>
        public FunderMapsRole(string roleName)
            : base(roleName)
        {
        }
    }
}
