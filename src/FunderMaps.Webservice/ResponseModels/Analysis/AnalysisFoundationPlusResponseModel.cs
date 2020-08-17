using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels.Analysis
{
    /// <summary>
    /// Represents a response model for the foundation plus endpoint.
    /// </summary>
    public sealed class AnalysisFoundationPlusResponseModel : AnalysisFoundationResponseModel
    {
        /// <summary>
        /// Complete description of this building.
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// Represents the reliability of all data about this building.
        /// </summary>
        public ReliabilityResponseModel? Reliability { get; set; }

        /// <summary>
        /// Represents the distribution of foundation types.
        /// </summary>
        public FoundationTypeDistributionResponseModel FoundationTypeDistribution { get; set; }

        /// <summary>
        /// Represents the distribution of building construction years in the
        /// given region.
        /// </summary>
        public ConstructionYearDistributionResponseModel ConstructionYearDistribution { get; set; }

        /// <summary>
        /// Represents the distribution of foundation risks in the given region.
        /// </summary>
        public FoundationRiskDistributionResponseModel FoundationRiskDistribution { get; set; }

        /// <summary>
        /// Represents the percentage of collected data in the given region.
        /// </summary>
        public double? DataCollectedPercentage { get; set; }

        /// <summary>
        /// Total amount of reports in the given region.
        /// </summary>
        public uint? TotalReportCount { get; set; }
    }
}
