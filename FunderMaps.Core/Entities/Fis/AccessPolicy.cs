using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities.Fis
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

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecovery> FoundationRecovery { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Report> Report { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Sample> Sample { get; set; }
    }
}
