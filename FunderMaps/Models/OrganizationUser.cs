using System;
using FunderMaps.Models.Identity;

namespace FunderMaps.Models
{
    public class OrganizationUser
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid OrganizationRoleId { get; set; }

        public FunderMapsUser User { get; set; }
        public Organization Organization { get; set; }
        public OrganizationRole OrganizationRole { get; set; }

        public OrganizationUser()
        {
        }

        public OrganizationUser(FunderMapsUser user, Organization organization)
        {
            User = user;
            Organization = organization;
        }

        public OrganizationUser(FunderMapsUser user, Organization organization, OrganizationRole role)
        {
            User = user;
            Organization = organization;
            OrganizationRole = role;
        }
    }
}
