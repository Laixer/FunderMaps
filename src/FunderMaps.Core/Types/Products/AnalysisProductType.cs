namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    ///     Enum representing the different FunderMaps analysis product types.
    /// </summary>
    public enum AnalysisProductType
    {
        /// <summary>
        ///     Represents all data about a given building itself.
        /// </summary>
        BuildingData,

        /// <summary>
        ///     Represents all data about a foundation for a given building.
        /// </summary>
        Foundation,

        /// <summary>
        ///     Represents all data about a foundation for a given building, including 
        ///     the details on which the data is based.
        /// </summary>
        FoundationPlus,

        /// <summary>
        ///     Represents all data about foundation restoration costs for a given building.
        /// </summary>
        Costs,

        /// <summary>
        ///     Represents a call with all possible data combined for a given building.
        /// </summary>
        Complete,

        /// <summary>
        ///     Represents descriptive texts for a given building.
        /// </summary>
        BuildingDescription,

        /// <summary>
        ///     Represents all data about the risks for a given building.
        /// </summary>
        Risk,
    }
}
