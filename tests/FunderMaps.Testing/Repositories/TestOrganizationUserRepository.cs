using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Testing.Repositories
{
    public class OrganizationUserRecord
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
        public OrganizationRole OrganizationRole { get; set; }
    }
}
