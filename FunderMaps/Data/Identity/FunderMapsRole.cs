using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FunderMaps.Data.Identity
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
