namespace FunderMaps.Webservice.ResponseModels.Building
{
    /// <summary>
    /// Represents a response model for the costs endpoint.
    /// </summary>
    public sealed class CostsResponseModel : BuildingResponseModelBase
    {
        /// <summary>
        /// Represents the foundation risk for this building.
        /// </summary>
        public double FoundationRisk { get; set; }

        /// <summary>
        /// Represents the estimated restoration costs for this building.
        /// TODO Correct unit?
        /// </summary>
        public double RestorationCosts { get; set; }

        /// <summary>
        /// Represents the reliability of all data about this building.
        /// TODO What unit? Percentage?
        /// TODO Correct name?
        /// </summary>
        public double Reliability { get; set; }

        /// <summary>
        /// Total amount of restored buildings in the given area.
        /// </summary>
        public uint TotalBuildingRestoredCount { get; set; }

        /// <summary>
        /// Total amount of incidents in the given region.
        /// </summary>
        public uint TotalIncidentCount { get; set; }
    }
}
