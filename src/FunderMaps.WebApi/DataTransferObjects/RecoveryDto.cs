using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    // TODO: Missing AuditStatus?
    /// <summary>
    ///     Recovery DTO.
    /// </summary>
    public sealed class RecoveryDto
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

        /// <summary>
        ///     Record delete date.
        /// </summary>
        public DateTime? DeleteDate { get; set; }
    }
}
