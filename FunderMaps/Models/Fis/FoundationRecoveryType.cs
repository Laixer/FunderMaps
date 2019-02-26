using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public class FoundationRecoveryType
    {
        public FoundationRecoveryType()
        {
            FoundationRecovery = new HashSet<FoundationRecovery>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        public ICollection<FoundationRecovery> FoundationRecovery { get; set; }
    }
}
