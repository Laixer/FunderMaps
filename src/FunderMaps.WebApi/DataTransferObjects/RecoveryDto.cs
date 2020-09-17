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
