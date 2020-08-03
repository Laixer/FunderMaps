namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    /// Enum representing the different FunderMaps statistics product types.
    /// </summary>
    public enum StatisticsProductType
    {
        /// <summary>
        /// Area analysis on the ratio of foundation types.
        /// </summary>
        StatisticsFoundationRatio,

        /// <summary>
        /// Area analysis on the distribution of built years.
        /// </summary>
        StatisticsConstructionYears,

        /// <summary>
        /// Area analysis on building foundation risk.
        /// </summary>
        StatisticsFoundationRisk,

        /// <summary>
        /// Area analysis on the percentage of data collected.
        /// </summary>
        StatisticsDataCollected,

        /// <summary>
        /// Area analysis on the amount of restored buildings.
        /// </summary>
        StatisticsBuildingsRestored,

        /// <summary>
        /// Area analysis on the amount of incidents.
        /// </summary>
        StatisticsIncidents,

        /// <summary>
        /// Area analysis on the amount of reports.
        /// </summary>
        StatisticsReports,
    }
}
