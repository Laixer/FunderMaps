using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    public class FoundationRecoveryEvidence : RecordControl
    {
        [MaxLength(96)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Document { get; set; }
        public string Note { get; set; }
        public FoundationRecoveryEvidenceType Type { get; set; }
        public int Recovery { get; set; }

        public virtual FoundationRecovery RecoveryNavigation { get; set; }
    }
}
