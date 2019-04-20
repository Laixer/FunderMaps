using System;
using System.Collections.Generic;

namespace FunderMaps.Core.Entities.Fis
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
