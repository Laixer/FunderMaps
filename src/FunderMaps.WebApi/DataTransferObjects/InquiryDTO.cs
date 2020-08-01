using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Inquiry DTO.
    /// </summary>
    public sealed class InquiryDto
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

        // TODO: Check if starts with https://
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
        ///     Conforms the F3O standaard.
        /// </summary>
        public bool StandardF3o { get; set; }

        /// <summary>
        ///     Attribution key.
        /// </summary>
        public int Attribution { get; set; }

        /// <summary>
        ///     Record access policy.
        /// </summary>
        public AccessPolicy AccessPolicy { get; set; }

        /// <summary>
        ///     Record create date.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Record last update.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        ///     Record delete date.
        /// </summary>
        public DateTime? DeleteDate { get; set; }
    }
}
