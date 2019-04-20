using System;

namespace FunderMaps.Core.Entities.Fis
{
    public class FoundationRecoveryEvidence : RecordControl
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
        public int Recovery { get; set; }

        public virtual FoundationRecovery RecoveryNavigation { get; set; }
        public virtual FoundationRecoveryEvidenceType TypeNavigation { get; set; }
    }
}
