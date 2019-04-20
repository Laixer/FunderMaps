using System;
using System.Collections.Generic;

namespace FunderMaps.Core.Entities.Fis
{
    public class FoundationRecoveryType
    {
        public FoundationRecoveryType()
        {
            FoundationRecovery = new HashSet<FoundationRecovery>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        public virtual ICollection<FoundationRecovery> FoundationRecovery { get; set; }
    }
}
