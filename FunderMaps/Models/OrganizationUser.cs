using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Models.Identity;

namespace FunderMaps.Models
{
    public class OrganizationUser
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }

        public FunderMapsUser User { get; set; }
        public Organization Organization { get; set; }
    }
}
