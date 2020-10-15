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
        BuildingData = 0,

        /// <summary>
        ///     Represents all data about a foundation for a given building.
        /// </summary>
        Foundation = 1,

        /// <summary>
        ///     Represents all data about a foundation for a given building, including 
        ///     the details on which the data is based.
        /// </summary>
        FoundationPlus = 2,

        /// <summary>
        ///     Represents all data about foundation restoration costs for a given building.
        /// </summary>
        Costs = 3,

        /// <summary>
        ///     Represents a call with all possible data combined for a given building.
        /// </summary>
        Complete = 4,

        /// <summary>
        ///     Represents all data about the risks for a given building.
        /// </summary>
        Risk = 5,

#if DEBUG || USE_BUILDINGDESCRIPTION // NOTE: This is a future feature.
        /// <summary>
        ///     Represents descriptive texts for a given building.
        /// </summary>
        BuildingDescription = 6,
#endif
    }
}
