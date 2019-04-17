using System;
using System.Runtime.Serialization;

namespace FunderMaps.Models
{
    public class OrganizationRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [IgnoreDataMember]
        public string NormalizedName { get; set; }

        [IgnoreDataMember]
        public string ConcurrencyStamp { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
