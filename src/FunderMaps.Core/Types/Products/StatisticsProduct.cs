using FunderMaps.Core.Types.Distributions;

namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    /// Model representing statistics about some region.
    /// </summary>
    public sealed class StatisticsProduct : ProductBase
    {
        /// <summary>
        ///     Internal neighborhood id in which these statistics were calculated.
        /// </summary>
        public string NeighborhoodId { get; set; }

        /// <summary>
        ///     Neighborhood code in which these statistics were calculated.
        /// </summary>
        public string NeighborhoodCode { get; set; }

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
        public double? DataCollectedPercentage { get; set; }

        /// <summary>
        /// Total amount of restored buildings in the given area.
        /// </summary>
        public uint? TotalBuildingRestoredCount { get; set; }

        /// <summary>
        /// Total amount of incidents in the given region.
        /// </summary>
        public uint? TotalIncidentCount { get; set; }

        /// <summary>
        /// Total amount of reports in the given region.
        /// </summary>
        public uint? TotalReportCount { get; set; }
    }
}
