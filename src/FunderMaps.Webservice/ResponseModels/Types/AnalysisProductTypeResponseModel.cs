namespace FunderMaps.Webservice.ResponseModels.Types
{
    /// <summary>
    ///     Enum representing analysis products.
    /// </summary>
    public enum AnalysisProductTypeResponseModel
    {
        /// <summary>
        /// Represents all data about a given building itself.
        /// </summary>
        BuildingData,

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
        /// Represents all data about the risks for a given building.
        /// </summary>
        Risk,
    }
}
