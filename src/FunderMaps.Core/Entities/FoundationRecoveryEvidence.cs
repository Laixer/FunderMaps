using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;
#if KAAS
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
        public Recovery RecoveryNavigation { get; set; }
    }
}
#endif