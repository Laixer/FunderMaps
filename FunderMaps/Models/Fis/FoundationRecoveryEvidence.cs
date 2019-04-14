using System;

namespace FunderMaps.Models.Fis
{
    public class FoundationRecoveryEvidence
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string Type { get; set; }
        public int Recovery { get; set; }

        public virtual FoundationRecovery RecoveryNavigation { get; set; }
        public virtual FoundationRecoveryEvidenceType TypeNavigation { get; set; }
    }
}
