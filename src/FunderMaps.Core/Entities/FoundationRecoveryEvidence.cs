using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Foundation recovery evidence.
    /// </summary>
    public class FoundationRecoveryEvidence : RecordControl
    {
        /// <summary>
        /// Name
        /// </summary>
        [MaxLength(96)]
        public string Name { get; set; }

        /// <summary>
        /// Document.
        /// </summary>
        [MaxLength(256)]
        public string Document { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Evidence type.
        /// </summary>
        public FoundationRecoveryEvidenceType Type { get; set; }

        /// <summary>
        /// Recovery identifier.
        /// </summary>
        public int Recovery { get; set; }

        /// <summary>
        /// Recovery navigation.
        /// </summary>
        public FoundationRecovery RecoveryNavigation { get; set; }
    }
}
