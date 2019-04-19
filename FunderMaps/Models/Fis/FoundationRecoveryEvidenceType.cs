using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public class FoundationRecoveryEvidenceType
    {
        public FoundationRecoveryEvidenceType()
        {
            FoundationRecoveryEvidence = new HashSet<FoundationRecoveryEvidence>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        public virtual ICollection<FoundationRecoveryEvidence> FoundationRecoveryEvidence { get; set; }
    }
}
