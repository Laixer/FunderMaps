namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    /// Enum representing the different FunderMaps analysis product types.
    /// </summary>
    public enum AnalysisProductType
    {
        /// <summary>
        /// Represents all data about a given building itself.
        /// </summary>
        AnalysisBuildingData,

        /// <summary>
        /// Represents all data about a foundation for a given building.
        /// </summary>
        AnalysisFoundation,

        /// <summary>
        /// Represents all data about a foundation for a given building, including 
        /// the details on which the data is based.
        /// </summary>
        AnalysisFoundationPlus,

        /// <summary>
        /// Represents all data about foundation restoration costs for a given building.
        /// </summary>
        AnalysisCosts,

        /// <summary>
        /// Represents a call with all possible data combined for a given building.
        /// </summary>
        AnalysisComplete,

        /// <summary>
        /// Represents descriptive texts for a given building.
        /// </summary>
        AnalysisBuildingDescription,

        /// <summary>
        /// Represents all data about the risks for a given building.
        /// </summary>
        AnalysisRisk,
    }
}
