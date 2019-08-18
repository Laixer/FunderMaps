using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities.Fis
{
    /// <summary>
    /// Report entity.
    /// </summary>
    public class Report2 : AccessControl
    {
        // TODO: FOR NOW
        [IgnoreDataMember]
        public int Attribution { get; set; }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string DocumentId { get; set; }

        public bool Inspection { get; set; }
        public bool JointMeasurement { get; set; }
        public bool FloorMeasurement { get; set; }

        public string Note { get; set; }

        public ReportStatus Status { get; set; }

        public ReportType Type { get; set; }

        [Required]
        public DateTime DocumentDate { get; set; }

        [MaxLength(256)]
        public string DocumentName { get; set; }

        [IgnoreDataMember]
        public int _Attribution { get; set; }

        //[Required]
        //public Attribution Attribution { get; set; }

        public virtual Norm Norm { get; set; }
    }
}
