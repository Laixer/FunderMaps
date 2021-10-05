using FunderMaps.Core.Types;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Represents a response model for the complete endpoint.
    /// </summary>
    public record AnalysisV2Dto : AnalysisBaseDto
    {
        /// <summary>
        ///     Represents the year in which this building was built.
        /// </summary>
        public int ConstructionYear { get; set; }

        /// <summary>
        ///     Foundation recovery type.
        /// </summary>
        public RecoveryType? RecoveryType { get; set; }

        /// <summary>
        ///     Represents the estimated restoration costs for this building.
        /// </summary>
        public int? RestorationCosts { get; set; }

        /// <summary>
        ///     Foundation type.
        /// </summary>
        public FoundationType? FoundationType { get; set; }

        /// <summary>
        ///     Foundation type reliability.
        /// </summary>
        public Reliability FoundationTypeReliability { get; set; }

        /// <summary>
        ///     Foundation type reliability.
        /// </summary>
        public Reliability DrystandReliability { get; set; }

        /// <summary>
        ///     Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk? DrystandRisk { get; set; }

        /// <summary>
        ///     Dewatering depth reliability.
        /// </summary>
        public Reliability DewateringDepthReliability { get; set; }

        /// <summary>
        ///     Dewatering depth risk.
        /// </summary>
        public FoundationRisk? DewateringDepthRisk { get; set; }

        /// <summary>
        ///     Biological infection reliability.
        /// </summary>
        public Reliability BioInfectionReliability { get; set; }

        /// <summary>
        ///     Biological infection risk.
        /// </summary>
        public FoundationRisk? BioInfectionRisk { get; set; }
    }
}
