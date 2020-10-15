using FunderMaps.Core.Types;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    // FUTURE: This is a copy of FunderMaps.Webservice.ResponseModels.Analysis.AnalysisRiskResponseModel, maybe move?
    /// <summary>
    ///     Represents a response model for the risk endpoint.
    /// </summary>
    public sealed class AnalysisRiskDto
    {
        /// <summary>
        ///     Internal neighborhood id in which this building lies.
        /// </summary>
        public string NeighborhoodId { get; set; }

        /// <summary>
        ///     Represents the foundation type of this building.
        /// </summary>
        public FoundationType FoundationType { get; set; }

        /// <summary>
        ///     Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk FoundationRisk { get; set; }

        /// <summary>
        ///     Represents the estimated restoration costs for this building.
        /// </summary>
        public double? RestorationCosts { get; set; }

        /// <summary>
        ///     Represents the dewatering depth (ontwateringsdiepte) for this building.
        /// </summary>
        public double? DewateringDepth { get; set; }

        /// <summary>
        ///     Represents the drystand (droogstand) for this building.
        /// </summary>
        public double? Drystand { get; set; }

        /// <summary>
        ///     Represents the reliability of all data about this building.
        /// </summary>
        public Reliability Reliability { get; set; }
    }
}
