using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Control;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Inquiry base entity.
    /// </summary>
    public class InquiryBase<TParent> : IdentifiableEntity<TParent, int>
        where TParent : class
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryBase(Func<TParent, int> entryPrimaryKey)
            : base(entryPrimaryKey)
        {
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Client document identifier.
        /// </summary>
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
        [Required]
        public string DocumentFile { get; set; }

        /// <summary>
        ///     Report type.
        /// </summary>
        public InquiryType Type { get; set; }

        /// <summary>
        ///     Coforms the F3O standaard.
        /// </summary>
        public bool StandardF3o { get; set; }

        /// <summary>
        ///     Print object as name.
        /// </summary>
        /// <returns>String representing inquiry.</returns>
        public override string ToString() => DocumentName;

        /// <summary>
        ///     Initialize property defaults.
        /// </summary>
        public override void InitializeDefaults()
        {
            Id = 0;
        }
    }

    /// <summary>
    ///     Inquiry entity.
    /// </summary>
    public class Inquiry : InquiryBase<Inquiry>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public Inquiry()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Initialize properties from another entity.
        /// </summary>
        public override void InitializeDefaults(Inquiry other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Id = other.Id;
        }
    }

    /// <summary>
    ///     Inquiry full entity.
    /// </summary>
    public sealed class InquiryFull : InquiryBase<InquiryFull>, IAttribution, IStateControl, IAccessControl, IRecordControl
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryFull()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Attribution control.
        /// </summary>
        public AttributionControl Attribution { get; set; }

        /// <summary>
        ///     State control.
        /// </summary>
        public StateControl State { get; set; }

        /// <summary>
        ///     Access control.
        /// </summary>
        public AccessControl Access { get; set; }

        /// <summary>
        ///     Record control.
        /// </summary>
        public RecordControl Record { get; set; }

        /// <summary>
        ///     Initialize properties from another entity.
        /// </summary>
        public override void InitializeDefaults(InquiryFull other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Id = other.Id;
        }
    }
}
