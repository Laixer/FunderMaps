using FunderMaps.Webservice.Types;

namespace FunderMaps.Webservice.Core.Models.Building
{
    /// <summary>
    /// Represents a model for the foundation plus endpoint.
    /// </summary>
    public sealed class BuildingFoundationPlus : BuildingBase
    {
        /// <summary>
        /// Complete description of this building.
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// Represents the reliability of all data about this building.
        /// TODO What unit? Percentage?
        /// TODO Correct name?
        /// </summary>
        public double Reliability { get; set; }

        /// <summary>
        /// Represents the distribution of foundation types.
        /// </summary>
        public FoundationTypeDistribution FoundationTypeDistribution { get; set; }

        /// <summary>
        /// Represents the distribution of building construction years in the
        /// given region.
        /// </summary>
        public ConstructionYearDistribution ConstructionYearDistribution { get; set; }

        /// <summary>
        /// Represents the distribution of foundation risks in the given region.
        /// </summary>
        public FoundationRiskDistribution FoundationRiskDistribution { get; set; }

        /// <summary>
        /// Represents the percentage of collected data in the given region.
        /// </summary>
        public double DataCollectedPercentage { get; set; }

        /// <summary>
        /// Total amount of reports in the given region.
        /// </summary>
        public uint TotalReportCount { get; set; }
    }
}
