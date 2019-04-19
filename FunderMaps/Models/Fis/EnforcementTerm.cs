using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public partial class EnforcementTerm
    {
        public EnforcementTerm()
        {
            Sample = new HashSet<Sample>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Sample> Sample { get; set; }
    }
}
