using System;
using Microsoft.AspNetCore.Identity;

namespace FunderMaps.Models.Identity
{
    public class FunderMapsRole : IdentityRole<Guid>
    {
        public FunderMapsRole()
        {
        }

        public FunderMapsRole(string roleName)
            : base(roleName)
        {
        }
    }
}
