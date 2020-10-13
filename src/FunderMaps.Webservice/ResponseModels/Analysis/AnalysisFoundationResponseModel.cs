using FunderMaps.Core.Types;

namespace FunderMaps.Webservice.ResponseModels.Analysis
{
    /// <summary>
    ///     Represents a response model for the foundation endpoint.
    /// </summary>
    public class AnalysisFoundationResponseModel : AnalysisResponseModelBase
    {
        /// <summary>
        ///     Represents the foundation type of this building.
        /// </summary>
        public FoundationType FoundationType { get; set; }

        /// <summary>
        ///     Represents the ground water level.
        /// </summary>
        public double? GroundWaterLevel { get; set; }

        /// <summary>
        ///     Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk FoundationRisk { get; set; }

        /// <summary>
        ///     Represents the ground level (maaiveldniveau) of this building.
        /// </summary>
        public double? GroundLevel { get; set; }

        /// <summary>
        ///     Represents the dewatering depth (ontwateringsdiepte) for this building.
        /// </summary>
        public double? DewateringDepth { get; set; }

        /// <summary>
        ///     Represents the drystand (droogstand) for this building.
        /// </summary>
        public double? Drystand { get; set; }
    }
}
