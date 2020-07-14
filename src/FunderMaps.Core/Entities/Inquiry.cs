using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Inquiry entity.
    /// </summary>
    public class Inquiry : AttributionControl
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Client document identifier.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string DocumentName { get; set; }

        /// <summary>
        ///     Inspection.
        /// </summary>
        public bool Inspection { get; set; }

        /// <summary>
        ///     Joint measurement.
        /// </summary>
        public bool JointMeasurement { get; set; }

        /// <summary>
        ///     Floor measurement.
        /// </summary>
        public bool FloorMeasurement { get; set; }

        /// <summary>
        ///     Note.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        /// <summary>
        ///     Original document creation.
        /// </summary>
        [DataType(DataType.DateTime)]
        [Required, Range(typeof(DateTime), "01/01/1000", "01/01/2100")]
        public DateTime DocumentDate { get; set; }

        /// <summary>
        ///     Document file name.
        /// </summary>
        [Required, Url]
        public string DocumentFile { get; set; }

        /// <summary>
        ///     Report status.
        /// </summary>
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        ///     Report type.
        /// </summary>
        public InquiryType Type { get; set; }

        /// <summary>
        ///     Coforms the F3O standaard.
        /// </summary>
        public bool StandardF3o { get; set; }
    }
}
