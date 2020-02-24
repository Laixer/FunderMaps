using FunderMaps.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.ViewModels
{
    /// <summary>
    /// Geocoder.
    /// </summary>
    public sealed class GeocoderAddress
    {
        /// <summary>
        /// Street name for backwards compatibility.
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Building number for backwards compatibility.
        /// </summary>
        public int BuildingNumber { get; set; }

        /// <summary>
        /// BAG id.
        /// </summary>
        public string Bag { get; set; }

        /// <summary>
        /// Additional identifiers.
        /// </summary>
        public dynamic Additional { get; set; }
    }

    public sealed class SampleInputOutputModel
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Link to report.
        /// </summary>
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

        /// <summary>
        /// Name of monitoring well.
        /// </summary>
        [MaxLength(32)]
        public string MonitoringWell { get; set; }

        /// <summary>
        /// Name of CPT measurement.
        /// </summary>
        [MaxLength(32)]
        public string Cpt { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Wood measurement level.
        /// </summary>
        public decimal? WoodLevel { get; set; }

        /// <summary>
        /// Ground water level.
        /// </summary>
        public decimal? GroundwaterLevel { get; set; }

        /// <summary>
        /// Ground level.
        /// </summary>
        public decimal? GroundLevel { get; set; }

        /// <summary>
        /// If foundation recovery was advised.
        /// </summary>
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
        /// Associated address.
        /// </summary>
        [Required]
        public GeocoderAddress Address { get; set; }
    }
}
