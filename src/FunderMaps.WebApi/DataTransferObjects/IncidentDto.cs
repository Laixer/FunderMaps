using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Incident DTO.
    /// </summary>
    public sealed class IncidentDto
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        [Incident]
        public string Id { get; set; }

        /// <summary>
        ///     Client identifier.
        /// </summary>
        [Required, Range(1, 99)]
        public int ClientId { get; set; }

        // FUTURE: Rename to type
        /// <summary>
        ///     Foundation type.
        /// </summary>
        [EnumDataType(typeof(FoundationType))]
        public FoundationType FoundationType { get; set; }

        /// <summary>
        ///     Building chained to another building.
        /// </summary>
        public bool ChainedBuilding { get; set; }

        /// <summary>
        ///     Whether the contact is an owner of the building.
        /// </summary>
        public bool Owner { get; set; }

        /// <summary>
        ///     Whether foundation was recovered or not.
        /// </summary>
        public bool FoundationRecovery { get; set; }

        /// <summary>
        ///     Whether neighbor foundation was recovered or not.
        /// </summary>
        public bool NeighborRecovery { get; set; }

        /// <summary>
        ///     Foundation damage cause.
        /// </summary>
        [EnumDataType(typeof(FoundationDamageCause))]
        public FoundationDamageCause FoundationDamageCause { get; set; }

        /// <summary>
        ///     Document name.
        /// </summary>
        public string[] DocumentFile { get; set; }

        /// <summary>
        ///     Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        ///     Internal note.
        /// </summary>
        public string InternalNote { get; set; }

        /// <summary>
        ///     Audit status.
        /// </summary>
        [EnumDataType(typeof(AuditStatus))]
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        ///     Question type.
        /// </summary>
        [EnumDataType(typeof(IncidentQuestionType))]
        public IncidentQuestionType QuestionType { get; set; }

        /// <summary>
        ///     Foundational damage.
        /// </summary>
        public FoundationDamageCharacteristics[] FoundationDamageCharacteristics { get; set; }

        /// <summary>
        ///     Environmental damage.
        /// </summary>
        public EnvironmentDamageCharacteristics[] EnvironmentDamageCharacteristics { get; set; }

        /// <summary>
        ///     Contact email.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Contact name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Contact phone number.
        /// </summary>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Required, Geocoder]
        public string Address { get; set; }

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
