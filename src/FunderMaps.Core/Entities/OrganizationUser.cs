using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities.Fis
{
    /// <summary>
    /// Organizational user.
    /// </summary>
    public class OrganizationUser : BaseEntity
    {
        /// <summary>
        /// User Identifier.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Organization identifier.
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Role per user in this organization.
        /// </summary>
        public OrganizationRole Role { get; set; }

        /// <summary>
        /// User.
        /// </summary>
        [IgnoreDataMember]
        public object User { get; set; } // TODO: Should be FunderMapsUser

        /// <summary>
        /// Organization.
        /// </summary>
        [IgnoreDataMember]
        public Organization Organization { get; set; }
    }
}
