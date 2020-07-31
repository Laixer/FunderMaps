namespace FunderMaps.Webservice.ResponseModels.Building
{
    /// <summary>
    /// Represents a response model for the description endpoint.
    /// </summary>
    public sealed class DescriptionResponseModel : BuildingResponseModelBase
    {
        /// <summary>
        /// Description of the terrain on which this building lies.
        /// TODO Correct name?
        /// </summary>
        public string TerrainDescription { get; set; }

        /// <summary>
        /// Complete description of this building.
        /// </summary>
        public string FullDescription { get; set; }
    }
}
