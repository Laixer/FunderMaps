using System;
using System.Collections.Generic;

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

        public virtual ICollection<Sample> Sample { get; set; }
    }
}
