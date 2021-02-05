using FunderMaps.Core.Types.Distributions;

namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    ///     Model representing statistics about some region.
    /// </summary>
    public sealed record StatisticsProduct
    {
        /// <summary>
        ///     Represents the distribution of foundation types.
        /// </summary>
        public FoundationTypeDistribution FoundationTypeDistribution { get; set; }

        /// <summary>
        ///     Represents the distribution of building construction years in the
        ///     given region.
        /// </summary>
        public ConstructionYearDistribution ConstructionYearDistribution { get; set; }

        /// <summary>
        ///     Represents the distribution of foundation risks in the given region.
        /// </summary>
        public FoundationRiskDistribution FoundationRiskDistribution { get; set; }

        /// <summary>
        ///     Represents the percentage of collected data in the given region.
        /// </summary>
        public decimal DataCollectedPercentage { get; set; }

        /// <summary>
        ///     Total amount of restored buildings in the given area.
        /// </summary>
        public long TotalBuildingRestoredCount { get; set; }

        /// <summary>
        ///     Total amount of incidents in the given region.
        /// </summary>
        public long TotalIncidentCount { get; set; }

        /// <summary>
        ///     Total amount of reports in the given region.
        /// </summary>
        public long TotalReportCount { get; set; }
    }
}
