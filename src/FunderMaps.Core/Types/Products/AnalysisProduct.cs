using FunderMaps.Core.DataAnnotations;
using System;

namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    ///     Represents a model for the complete endpoint.
    /// </summary>
    public sealed class AnalysisProduct : ProductBase
    {
        /// <summary>
        ///     Building identifier.
        /// </summary>
        [Geocoder]
        public string Id { get; set; }

        /// <summary>
        ///     Building external identifier.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        ///     Postcode.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        ///     Represents the external data source of this building.
        /// </summary>
        public ExternalDataSource? ExternalSource { get; set; }

        /// <summary>
        ///     Built year.
        /// </summary>
        public DateTimeOffset ConstructionYear { get; set; }

        /// <summary>
        ///     Built year source.
        /// </summary>
        public BuiltYearSource ConstructionYearSource { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Geocoder]
        public string AddressId { get; set; }

        /// <summary>
        ///     Address external identifier.
        /// </summary>
        public string AddressExternalId { get; set; }

        /// <summary>
        ///     Neighborhood identifier.
        /// </summary>
        [Geocoder]
        public string NeighborhoodId { get; set; }

        /// <summary>
        ///     Represents the ground water level.
        /// </summary>
        public double? GroundWaterLevel { get; set; }

        /// <summary>
        ///     Soil code.
        /// </summary>
        public string Soil { get; set; }

        /// <summary>
        ///     Represents the height of this building.
        /// </summary>
        public double? BuildingHeight { get; set; }

        /// <summary>
        ///     Ground level in meters.
        /// </summary>
        public double? GroundLevel { get; set; }

        /// <summary>
        ///     Cone penetration test name.
        /// </summary>
        public string Cpt { get; set; }

        /// <summary>
        ///     Monitoring well.
        /// </summary>
        public string MonitoringWell { get; set; }

        /// <summary>
        ///     Recovery advised.
        /// </summary>
        public bool? RecoveryAdvised { get; set; }

        /// <summary>
        ///     Damage cause.
        /// </summary>
        public FoundationDamageCause? DamageCause { get; set; }

        /// <summary>
        ///     Substructure.
        /// </summary>
        public Substructure? Substructure { get; set; }

        /// <summary>
        ///     Client document identifier.
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        ///     Original document creation.
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        ///     Report type.
        /// </summary>
        public InquiryType? InquiryType { get; set; }

        /// <summary>
        ///     Foundation recovery type.
        /// </summary>
        public RecoveryDocumentType? RecoveryType { get; set; }

        /// <summary>
        ///     Foundation recovery status.
        /// </summary>
        public RecoveryStatus? RecoveryStatus { get; set; }

        /// <summary>
        ///     Building surface area in square meters.
        /// </summary>
        public double? SurfaceArea { get; set; }

        /// <summary>
        ///     Address living surface area in square meters.
        /// </summary>
        public double? LivingArea { get; set; }

        /// <summary>
        ///     Foundation bearing ground layer.
        /// </summary>
        public double? FoundationBearingLayer { get; set; }

        /// <summary>
        ///     Represents the estimated restoration costs for this building.
        /// </summary>
        public double? RestorationCosts { get; set; }

        /// <summary>
        ///     Description for restoration costs.
        /// </summary>
        public string DescriptionRestorationCosts { get; set; }

        /// <summary>
        ///     Foundation type.
        /// </summary>
        public FoundationType? FoundationType { get; set; }

        /// <summary>
        ///     Foundation type reliability.
        /// </summary>
        public Reliability FoundationTypeReliability { get; set; }

        /// <summary>
        ///     Represents the period of drought (droogstand) for this building.
        /// </summary>
        public double? Drystand { get; set; }

        /// <summary>
        ///     Foundation type reliability.
        /// </summary>
        public Reliability DrystandReliability { get; set; }

        /// <summary>
        ///     Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk? DrystandRisk { get; set; }

        /// <summary>
        ///     Description for drystand.
        /// </summary>
        public string DescriptionDrystand { get; set; }

        /// <summary>
        ///     Dewatering depth.
        /// </summary>
        public double? DewateringDepth { get; set; }

        /// <summary>
        ///     Dewatering depth reliability.
        /// </summary>
        public Reliability DewateringDepthReliability { get; set; }

        /// <summary>
        ///     Dewatering depth risk.
        /// </summary>
        public FoundationRisk? DewateringDepthRisk { get; set; }

        /// <summary>
        ///     Description for dewatering depth.
        /// </summary>
        public string DescriptionDewateringDepth { get; set; }

        /// <summary>
        ///     Biological infection.
        /// </summary>
        public string BioInfection { get; set; }

        /// <summary>
        ///     Biological infection reliability.
        /// </summary>
        public Reliability BioInfectionReliability { get; set; }

        /// <summary>
        ///     Biological infection risk.
        /// </summary>
        public FoundationRisk? BioInfectionRisk { get; set; }

        /// <summary>
        ///     Description for biological infection.
        /// </summary>
        public string DescriptionBioInfection { get; set; }

        /// <summary>
        ///     Statistisch per region.
        /// </summary>
        public StatisticsProduct Statistics { get; set; }
    }
}
