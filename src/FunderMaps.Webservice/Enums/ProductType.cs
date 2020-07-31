namespace FunderMaps.Webservice.Enums
{
    /// <summary>
    /// Enum representing the different FunderMaps product types.
    /// // TODO TO CORE
    /// </summary>
    public enum ProductType
    {
        /// <summary>
        /// Represents all data about a given building itself.
        /// </summary>
        Building,

        /// <summary>
        /// Represents all data about a foundation for a given building.
        /// </summary>
        Foundation,

        /// <summary>
        /// Represents all data about a foundation for a given building, including 
        /// the details on which the data is based.
        /// </summary>
        FoundationPlus,

        /// <summary>
        /// Represents all data about foundation restoration costs for a given building.
        /// </summary>
        Costs,

        /// <summary>
        /// Represents a call with all possible data combined for a given building.
        /// </summary>
        Complete,

        /// <summary>
        /// Represents descriptive texts for a given building.
        /// </summary>
        Description,

        /// <summary>
        /// Represents all data about the risks for a given building.
        /// </summary>
        Risk,

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
        StatisticsReports
    }
}
