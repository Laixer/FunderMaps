using FunderMaps.Core.DataAnnotations;
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
        /// <remarks>
        ///     This is the document file name as the user gave it, *not* 
        ///     the filename under which the document is stored. For that,
        ///     see <see cref="DocumentFile"/>.
        /// </remarks>
        [Required]
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
        /// <remarks>
        ///     This is the document file name as it is stored, NOT the document
        ///     name that the user gave it. <seealso cref="DocumentName"/>.
        /// </remarks>
        [Required]
        public string DocumentFile { get; set; }

        /// <summary>
        ///     Report type.
        /// </summary>
        public InquiryType Type { get; set; }

        /// <summary>
        ///     Conforms the F3O standaard.
        /// </summary>
        public bool StandardF3o { get; set; }

        /// <summary>
        ///     Report status.
        /// </summary>
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        ///     Reviewer identifier.
        /// </summary>
        public Guid? Reviewer { get; set; }

        /// <summary>
        ///     Creator identifier.
        /// </summary>
        public Guid Creator { get; set; }

        /// <summary>
        ///     Owner identifier.
        /// </summary>
        public Guid Owner { get; set; }

        /// <summary>
        ///     Contractor identifier.
        /// </summary>
        public Guid Contractor { get; set; }

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
    }
}
