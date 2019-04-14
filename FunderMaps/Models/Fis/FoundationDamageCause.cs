using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
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

        public virtual ICollection<Incident> Incident { get; set; }
        public virtual ICollection<Sample> Sample { get; set; }
    }
}
