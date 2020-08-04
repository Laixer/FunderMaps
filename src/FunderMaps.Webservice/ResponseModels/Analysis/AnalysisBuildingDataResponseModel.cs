using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels.Analysis
{
    /// <summary>
    /// Represents a response model for the building data endpoint.
    /// </summary>
    public sealed class AnalysisResponseModel : AnalysisResponseModelBase
    {
        /// <summary>
        /// Represents the foundation type of this building.
        /// </summary>
        public string FoundationType { get; set; }

        /// <summary>
        /// Represents the <see cref="Year"/> in which this building was built.
        /// </summary>
        public YearResponseModel ConstructionYear { get; set; }

        /// <summary>
        /// Represents the height of this building.
        /// </summary>
        public double BuildingHeight { get; set; }
    }
}
