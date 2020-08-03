namespace FunderMaps.Webservice.Core.Models.Building
{
    /// <summary>
    /// Represents a model for the description endpoint.
    /// </summary>
    public sealed class BuildingDescription : BuildingBase
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
