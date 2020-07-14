using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Foundation recovery entity.
    /// </summary>
    public class Recovery : AttributionControl
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Foundation recovery type.
        /// </summary>
        [Required]
        public RecoveryDocumentType Type { get; set; }

        /// <summary>
        /// Document file name.
        /// </summary>
        [Required]
        public string DocumentFile { get; set; }

        /// <summary>
        /// Document date.
        /// </summary>
        [Required]
        public DateTime DocumentDate { get; set; }
    }
}
