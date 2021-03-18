using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Incident DTO.
    /// </summary>
    public sealed record IncidentDto
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        [Incident]
        public string Id { get; init; }

        // FUTURE: Rename to type
        /// <summary>
        ///     Foundation type.
        /// </summary>
        [EnumDataType(typeof(FoundationType))]
        public FoundationType? FoundationType { get; init; }

        /// <summary>
        ///     Building chained to another building.
        /// </summary>
        public bool ChainedBuilding { get; init; }

        /// <summary>
        ///     Whether the contact is an owner of the building.
        /// </summary>
        public bool Owner { get; init; }

        /// <summary>
        ///     Whether foundation was recovered or not.
        /// </summary>
        public bool FoundationRecovery { get; init; }

        /// <summary>
        ///     Whether neighbor foundation was recovered or not.
        /// </summary>
        public bool NeighborRecovery { get; init; }

        /// <summary>
        ///     Foundation damage cause.
        /// </summary>
        [EnumDataType(typeof(FoundationDamageCause))]
        public FoundationDamageCause? FoundationDamageCause { get; init; }

        /// <summary>
        ///     Document name.
        /// </summary>
        public string[] DocumentFile { get; init; }

        /// <summary>
        ///     Note.
        /// </summary>
        public string Note { get; init; }

        /// <summary>
        ///     Internal note.
        /// </summary>
        public string InternalNote { get; init; }

        /// <summary>
        ///     Audit status.
        /// </summary>
        [EnumDataType(typeof(AuditStatus))]
        public AuditStatus AuditStatus { get; init; }

        /// <summary>
        ///     Question type.
        /// </summary>
        [EnumDataType(typeof(IncidentQuestionType))]
        public IncidentQuestionType QuestionType { get; init; }

        /// <summary>
        ///     Foundational damage.
        /// </summary>
        [ArrayEnumDataTypeAttribute(typeof(FoundationDamageCharacteristics))]
        public FoundationDamageCharacteristics[] FoundationDamageCharacteristics { get; init; }

        /// <summary>
        ///     Environmental damage.
        /// </summary>
        [ArrayEnumDataTypeAttribute(typeof(EnvironmentDamageCharacteristics))]
        public EnvironmentDamageCharacteristics[] EnvironmentDamageCharacteristics { get; init; }

        /// <summary>
        ///     Contact email.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; init; }

        /// <summary>
        ///     Contact name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        ///     Contact phone number.
        /// </summary>
        [Phone]
        [StringLength(16)]
        public string PhoneNumber { get; init; }

        /// <summary>
        ///     An address identifier.
        /// </summary>
        [Required]
        public string Address { get; init; }

        /// <summary>
        ///     Record create date.
        /// </summary>
        public DateTime CreateDate { get; init; }

        /// <summary>
        ///     Record last update.
        /// </summary>
        public DateTime? UpdateDate { get; init; }

        /// <summary>
        ///     Record delete date.
        /// </summary>
        public DateTime? DeleteDate { get; init; }
    }
}
