using System;

namespace FunderMaps.Models
{
    public class OrganizationRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
