namespace FunderMaps.Core.Types.Products;

/// <summary>
///     Enum representing the different FunderMaps statistics product types.
/// </summary>
public enum StatisticsProductType
{
    /// <summary>
    ///     Area analysis on the ratio of foundation types.
    /// </summary>
    FoundationRatio = 0,

    /// <summary>
    ///     Area analysis on the distribution of built years.
    /// </summary>
    ConstructionYears = 1,

    /// <summary>
    ///     Area analysis on building foundation risk.
    /// </summary>
    FoundationRisk = 2,

    /// <summary>
    ///     Area analysis on the percentage of data collected.
    /// </summary>
    DataCollected = 3,

    /// <summary>
    ///     Area analysis on the amount of restored buildings.
    /// </summary>
    BuildingsRestored = 4,

    /// <summary>
    ///     Area analysis on the amount of incidents.
    /// </summary>
    Incidents = 5,

    /// <summary>
    ///     Area analysis on the amount of reports.
    /// </summary>
    Reports = 6,
}
