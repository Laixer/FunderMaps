using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels.Analysis
{
    /// <summary>
    /// Represents a response model for the risk endpoint.
    /// </summary>
    public sealed class AnalysisRiskResponseModel : AnalysisResponseModelBase
    {
        /// <summary>
        /// Represents the foundation type of this building.
        /// </summary>
        public FoundationTypeResponseModel FoundationType { get; set; }

        /// <summary>
        /// Represents the foundation risk for this building.
        /// </summary>
        public FoundationRiskResponseModel FoundationRisk { get; set; }

        /// <summary>
        /// Description of the terrain on which this building lies.
        /// </summary>
        public string TerrainDescription { get; set; }

        /// <summary>
        /// Represents the estimated restoration costs for this building.
        /// </summary>
        public double? RestorationCosts { get; set; }

        /// <summary>
        /// Represents the dewatering depth (ontwateringsdiepte) for this building.
        /// </summary>
        public double? DewateringDepth { get; set; }

        /// <summary>
        /// Represents the drystand (droogstand) for this building.
        /// </summary>
        public double? Drystand { get; set; }

        /// <summary>
        /// Represents the reliability of all data about this building.
        /// </summary>
        public ReliabilityResponseModel Reliability { get; set; }
    }
}
