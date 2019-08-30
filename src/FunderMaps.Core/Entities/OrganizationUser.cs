using System;
using System.Collections.Generic;
using System.Text;

namespace FunderMaps.Core.Entities.Fis
{
    public class OrganizationUser : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid OrganizationRoleId { get; set; }

        public object User { get; set; }
        public Organization Organization { get; set; }
        public OrganizationRole OrganizationRole { get; set; }

        public OrganizationUser()
        {
        }

        public OrganizationUser(object user, Organization organization)
        {
            User = user;
            Organization = organization;
        }

        public OrganizationUser(object user, Organization organization, OrganizationRole role)
        {
            User = user;
            Organization = organization;
            OrganizationRole = role;
        }
    }
}
