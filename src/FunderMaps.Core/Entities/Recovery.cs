using FunderMaps.Core.Entities.Report;
using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    // TODO inherit from StateControl?
    /// <summary>
    ///     Foundation recovery entity.
    /// </summary>
    public sealed class Recovery : AttributionControl<Recovery, int>, IReportEntity<Recovery>
    {
        public Recovery()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        ///     Foundation recovery type.
        /// </summary>
        [Required]
        public RecoveryDocumentType Type { get; set; }

        /// <summary>
        ///     Document file name.
        /// </summary>
        [Required]
        public string DocumentFile { get; set; }

        /// <summary>
        ///     Document date.
        /// </summary>
        [Required]
        public DateTime DocumentDate { get; set; }
    }
}
