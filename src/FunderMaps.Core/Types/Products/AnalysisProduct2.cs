using FunderMaps.Core.DataAnnotations;

namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    ///     Represents a model for the complete endpoint.
    /// </summary>
    public sealed record AnalysisProduct2
    {
        /// <summary>
        ///     Building identifier.
        /// </summary>
        [Geocoder]
        public string BuildingId { get; init; }

        /// <summary>
        ///     Building external identifier.
        /// </summary>
        public string ExternalBuildingId { get; init; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Geocoder]
        public string AddressId { get; init; }

        /// <summary>
        ///     Address external identifier.
        /// </summary>
        public string ExternalAddressId { get; init; }

        /// <summary>
        ///     Neighborhood identifier.
        /// </summary>
        [Geocoder]
        public string NeighborhoodId { get; init; }

        /// <summary>
        ///     Built year.
        /// </summary>
        public int? ConstructionYear { get; init; }

        /// <summary>
        ///     Foundation recovery type.
        /// </summary>
        public RecoveryType? RecoveryType { get; init; }

        /// <summary>
        ///     Represents the estimated restoration costs for this building.
        /// </summary>
        public int? RestorationCosts { get; init; }

        /// <summary>
        ///     Foundation type.
        /// </summary>
        public FoundationType? FoundationType { get; init; }

        /// <summary>
        ///     Foundation type reliability.
        /// </summary>
        public Reliability FoundationTypeReliability { get; init; }

        /// <summary>
        ///     Foundation type reliability.
        /// </summary>
        public Reliability DrystandReliability { get; init; }

        /// <summary>
        ///     Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk? DrystandRisk { get; init; }

        /// <summary>
        ///     Dewatering depth reliability.
        /// </summary>
        public Reliability DewateringDepthReliability { get; init; }

        /// <summary>
        ///     Dewatering depth risk.
        /// </summary>
        public FoundationRisk? DewateringDepthRisk { get; init; }

        /// <summary>
        ///     Biological infection reliability.
        /// </summary>
        public Reliability BioInfectionReliability { get; init; }

        /// <summary>
        ///     Biological infection risk.
        /// </summary>
        public FoundationRisk? BioInfectionRisk { get; init; }

        /// <summary>
        ///     Unclassified risk.
        /// </summary>
        public FoundationRisk? UnclassifiedRisk { get; init; }
    }
}
