using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public class AccessPolicy
    {
        public AccessPolicy()
        {
            FoundationRecovery = new HashSet<FoundationRecovery>();
            Report = new HashSet<Report>();
            Sample = new HashSet<Sample>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        public virtual ICollection<FoundationRecovery> FoundationRecovery { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<Sample> Sample { get; set; }
    }
}
