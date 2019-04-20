using System;
using System.Collections.Generic;

namespace FunderMaps.Core.Entities.Fis
{
    public class FoundationRecoveryLocation
    {
        public FoundationRecoveryLocation()
        {
            FoundationRecoveryRepair = new HashSet<FoundationRecoveryRepair>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        public virtual ICollection<FoundationRecoveryRepair> FoundationRecoveryRepair { get; set; }
    }
}
