using FunderMaps.Webservice.Types;

namespace FunderMaps.Webservice.Models.Statistics
{
    /// <summary>
    /// Model representing statistics about some region.
    /// </summary>
    public sealed class StatisticsEverything
    {
        /// <summary>
        /// The region in which these statistics were caluclated.
        /// </summary>
        public Region Region { get; set; }

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
        /// Total amount of restored buildings in the given area.
        /// </summary>
        public uint TotalBuildingRestored { get; set; }

        /// <summary>
        /// Total amount of incidents in the given region.
        /// </summary>
        public uint TotalIncidents { get; set; }

        /// <summary>
        /// Total amount of reports in the given region.
        /// </summary>
        public uint TotalReportCount { get; set; }
    }
}
