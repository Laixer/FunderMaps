using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities.Fis
{
    public class FoundationDamageCause
    {
        public FoundationDamageCause()
        {
            Incident = new HashSet<Incident>();
            Sample = new HashSet<Sample>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Incident> Incident { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Sample> Sample { get; set; }
    }
}
