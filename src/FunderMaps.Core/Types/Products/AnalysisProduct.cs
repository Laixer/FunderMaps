using FunderMaps.Core.DataAnnotations;
using System;

namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    ///     Represents a model for the complete endpoint.
    /// </summary>
    public sealed record AnalysisProduct
    {
        /// <summary>
        ///     Building identifier.
        /// </summary>
        [Geocoder]
        public string Id { get; init; }

        /// <summary>
        ///     Building external identifier.
        /// </summary>
        public string ExternalId { get; init; }

        /// <summary>
        ///     Postal code
        /// </summary>
        public string PostalCode { get; init; }

        /// <summary>
        ///     Represents the external data source of this building.
        /// </summary>
        public ExternalDataSource? ExternalSource { get; init; }

        /// <summary>
        ///     Built year.
        /// </summary>
        public DateTimeOffset ConstructionYear { get; init; }

        /// <summary>
        ///     Built year source.
        /// </summary>
        public BuiltYearSource ConstructionYearSource { get; init; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Geocoder]
        public string AddressId { get; init; }

        /// <summary>
        ///     Address external identifier.
        /// </summary>
        public string AddressExternalId { get; init; }

        /// <summary>
        ///     Neighborhood identifier.
        /// </summary>
        [Geocoder]
        public string NeighborhoodId { get; init; }

        /// <summary>
        ///     Represents the ground water level.
        /// </summary>
        public double? GroundWaterLevel { get; init; }

        /// <summary>
        ///     Soil code.
        /// </summary>
        public string Soil { get; init; }

        /// <summary>
        ///     Represents the height of this building.
        /// </summary>
        public double? BuildingHeight { get; init; }

        /// <summary>
        ///     Ground level in meters.
        /// </summary>
        public double? GroundLevel { get; init; }

        /// <summary>
        ///     Cone penetration test name.
        /// </summary>
        public string Cpt { get; init; }

        /// <summary>
        ///     Monitoring well.
        /// </summary>
        public string MonitoringWell { get; init; }

        /// <summary>
        ///     Recovery advised.
        /// </summary>
        public bool? RecoveryAdvised { get; init; }

        /// <summary>
        ///     Damage cause.
        /// </summary>
        public FoundationDamageCause? DamageCause { get; init; }

        /// <summary>
        ///     Substructure.
        /// </summary>
        public Substructure? Substructure { get; init; }

        /// <summary>
        ///     Client document identifier.
        /// </summary>
        public string DocumentName { get; init; }

        /// <summary>
        ///     Original document creation.
        /// </summary>
        public DateTime? DocumentDate { get; init; }

        /// <summary>
        ///     Report type.
        /// </summary>
        public InquiryType? InquiryType { get; init; }

        /// <summary>
        ///     Foundation recovery type.
        /// </summary>
        public RecoveryType? RecoveryType { get; init; }

        /// <summary>
        ///     Foundation recovery status.
        /// </summary>
        public RecoveryStatus? RecoveryStatus { get; init; }

        /// <summary>
        ///     Building surface area in square meters.
        /// </summary>
        public double? SurfaceArea { get; init; }

        /// <summary>
        ///     Address living surface area in square meters.
        /// </summary>
        public double? LivingArea { get; init; }

        /// <summary>
        ///     Foundation bearing ground layer.
        /// </summary>
        public double? FoundationBearingLayer { get; init; }

        /// <summary>
        ///     Represents the estimated restoration costs for this building.
        /// </summary>
        public double? RestorationCosts { get; init; }

        /// <summary>
        ///     Description for restoration costs.
        /// </summary>
        public string DescriptionRestorationCosts { get; init; }

        /// <summary>
        ///     Foundation type.
        /// </summary>
        public FoundationType? FoundationType { get; init; }

        /// <summary>
        ///     Foundation type reliability.
        /// </summary>
        public Reliability FoundationTypeReliability { get; init; }

        /// <summary>
        ///     Represents the period of drought (droogstand) for this building.
        /// </summary>
        public double? Drystand { get; init; }

        /// <summary>
        ///     Foundation type reliability.
        /// </summary>
        public Reliability DrystandReliability { get; init; }

        /// <summary>
        ///     Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk? DrystandRisk { get; init; }

        /// <summary>
        ///     Description for drystand.
        /// </summary>
        public string DescriptionDrystand { get; init; }

        /// <summary>
        ///     Dewatering depth.
        /// </summary>
        public double? DewateringDepth { get; init; }

        /// <summary>
        ///     Dewatering depth reliability.
        /// </summary>
        public Reliability DewateringDepthReliability { get; init; }

        /// <summary>
        ///     Dewatering depth risk.
        /// </summary>
        public FoundationRisk? DewateringDepthRisk { get; init; }

        /// <summary>
        ///     Description for dewatering depth.
        /// </summary>
        public string DescriptionDewateringDepth { get; init; }

        /// <summary>
        ///     Biological infection.
        /// </summary>
        public string BioInfection { get; init; }

        /// <summary>
        ///     Biological infection reliability.
        /// </summary>
        public Reliability BioInfectionReliability { get; init; }

        /// <summary>
        ///     Biological infection risk.
        /// </summary>
        public FoundationRisk? BioInfectionRisk { get; init; }

        /// <summary>
        ///     Description for biological infection.
        /// </summary>
        public string DescriptionBioInfection { get; init; }

        /// <summary>
        ///     Statistisch per region.
        /// </summary>
        public StatisticsProduct Statistics { get; init; }
    }
}
