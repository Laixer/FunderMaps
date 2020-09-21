using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities.Report;
using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    // TODO inherit from StateControl?
    /// <summary>
    ///     Indicent entity.
    /// </summary>
    public sealed class Incident : RecordControl<Incident, string>, IReportEntity<Incident>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public Incident()
            : base(e => e.Id)
        {
        }

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
        [ArrayUrl]
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
        ///     Fouindational damage.
        /// </summary>
        [ArrayEnumDataTypeAttribute(typeof(FoundationDamageCharacteristics))]
        public FoundationDamageCharacteristics[] FoundationDamageCharacteristics { get; set; }

        /// <summary>
        ///     Environmental damage.
        /// </summary>
        [ArrayEnumDataTypeAttribute(typeof(EnvironmentDamageCharacteristics))]
        public EnvironmentDamageCharacteristics[] EnvironmentDamageCharacteristics { get; set; }

        /// <summary>
        ///     Contact email.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Required, Geocoder]
        public string Address { get; set; }

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
        ///     Meta data.
        /// </summary>
        public object Meta { get; set; }

        /// <summary>
        ///     Print object as name.
        /// </summary>
        /// <returns>String representing incident.</returns>
        public override string ToString() => Id;

        /// <summary>
        ///     Initialize property defaults.
        /// </summary>
        public override void InitializeDefaults()
        {
            Id = null;
            AuditStatus = AuditStatus.Todo;
            CreateDate = DateTime.MinValue;
            UpdateDate = null;
            DeleteDate = null;
            AddressNavigation = null;
        }

        /// <summary>
        ///     Initialize properties from another entity.
        /// </summary>
        public override void InitializeDefaults(Incident other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Id = other.Id;
            AuditStatus = other.AuditStatus;
            CreateDate = other.CreateDate;
            UpdateDate = other.UpdateDate;
            DeleteDate = other.DeleteDate;
        }

        /// <summary>
        ///     Contact object.
        /// </summary>
        public Contact ContactNavigation { get; set; }

        /// <summary>
        ///     Address object.
        /// </summary>
        public Address AddressNavigation { get; set; }
    }
}
