using FunderMaps.Webservice.Enums;

namespace FunderMaps.Webservice.Core.Models.Building
{
    /// <summary>
    /// Represents a model for the costs endpoint.
    /// </summary>
    public sealed class BulidingCosts : BuildingBase
    {
        /// <summary>
        /// Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk FoundationRisk { get; set; }

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
