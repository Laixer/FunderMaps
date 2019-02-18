using System;
using FunderMaps.Models.Identity;

namespace FunderMaps.Models
{
    public class OrganizationUser
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? OrganizationRoleId { get; set; }

        public FunderMapsUser User { get; set; }
        public Organization Organization { get; set; }
        public OrganizationRole OrganizationRole { get; set; }
    }
}
