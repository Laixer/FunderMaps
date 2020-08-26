namespace FunderMaps.Webservice.ResponseModels.Analysis
{
    /// <summary>
    /// Represents a response model for the description endpoint.
    /// </summary>
    public sealed class AnalysisBuildingDescriptionResponseModel : AnalysisResponseModelBase
    {
        /// <summary>
        /// Description of the terrain on which this building lies.
        /// </summary>
        public string TerrainDescription { get; set; }

        /// <summary>
        /// Complete description of this building.
        /// </summary>
        public string FullDescription { get; set; }
    }
}
