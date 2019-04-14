using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public class Organization
    {
        public Organization()
        {
            AttributionContractorNavigation = new HashSet<Attribution>();
            AttributionOwnerNavigation = new HashSet<Attribution>();
            Principal = new HashSet<Principal>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Attribution> AttributionContractorNavigation { get; set; }
        public virtual ICollection<Attribution> AttributionOwnerNavigation { get; set; }
        public virtual ICollection<Principal> Principal { get; set; }
    }
}
