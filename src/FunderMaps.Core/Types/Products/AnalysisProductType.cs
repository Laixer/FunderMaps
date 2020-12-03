namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    ///     Enum representing the different FunderMaps analysis product types.
    /// </summary>
    public enum AnalysisProductType
    {
        /// <summary>
        ///     Represents all data about a foundation for a given building.
        /// </summary>
        Foundation = 1,

        /// <summary>
        ///     Represents a call with all possible data combined for a given building.
        /// </summary>
        Complete = 4,

        /// <summary>
        ///     Represents all data about the risks for a given building including source data.
        /// </summary>
        RiskPlus = 7,
    }
}
