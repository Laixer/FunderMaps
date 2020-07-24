using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Inquiry entity.
    /// </summary>
    public sealed class Inquiry : StateControl
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
        public void InitializeDefaults()
        {
            Id = 0;
            AuditStatus = AuditStatus.Todo;
            CreateDate = DateTime.MinValue;
            UpdateDate = null;
            DeleteDate = null;
            AttributionNavigation = null;
        }

        /// <summary>
        ///     Initialize properties from another entity.
        /// </summary>
        public void InitializeDefaults(Inquiry other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Id = other.Id;
            AuditStatus = other.AuditStatus;
            Attribution = other.Attribution;
            CreateDate = other.CreateDate;
            UpdateDate = other.UpdateDate;
            DeleteDate = other.DeleteDate;
        }
    }
}
