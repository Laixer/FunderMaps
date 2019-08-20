using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities.Fis
{
    /// <summary>
    /// Sample entity.
    /// </summary>
    public class Sample : AccessControl
    {
        // TODO: FOR NOW
        [IgnoreDataMember]
        public int Attribution { get; set; }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        [Required]
        public int? Report { get; set; }

        /// <summary>
        /// Foundation type.
        /// </summary>
        public FoundationType? FoundationType { get; set; }

        /// <summary>
        /// Foundation quality.
        /// </summary>
        public FoundationQuality? FoundationQuality { get; set; }

        /// <summary>
        /// Substructure.
        /// </summary>
        public Substructure? Substructure { get; set; }

        [MaxLength(32)]
        public string MonitoringWell { get; set; }
        [MaxLength(32)]
        public string Cpt { get; set; }
        public string Note { get; set; }
        public decimal? WoodLevel { get; set; }
        public decimal? GroundwaterLevel { get; set; }
        public decimal? GroundLevel { get; set; }
        public bool FoundationRecoveryAdviced { get; set; }

        /// <summary>
        /// Foundation damage cause.
        /// </summary>
        public FoundationDamageCause FoundationDamageCause { get; set; }

        /// <summary>
        /// Building built year.
        /// </summary>
        public short? BuiltYear { get; set; }

        /// <summary>
        /// Enforcement term.
        /// </summary>
        public EnforcementTerm? EnforcementTerm { get; set; }

        /// <summary>
        /// Level of base measurement.
        /// </summary>
        public BaseLevel BaseMeasurementLevel { get; set; }

        /// <summary>
        /// Address record identifier.
        /// </summary>
        [IgnoreDataMember]
        public Guid _Address { get; set; }

        /// <summary>
        /// Associated address.
        /// </summary>
        [Required]
        public Address2 Address { get; set; }
    }
}
