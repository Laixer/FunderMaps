using FunderMaps.Webservice.Enums;
using FunderMaps.Webservice.Types;

namespace FunderMaps.Webservice.Core.Models.Building
{
    /// <summary>
    /// Represents a model for the building data endpoint.
    /// </summary>
    public sealed class BuildingData : BuildingBase
    {
        /// <summary>
        /// Represents the foundation type of this building.
        /// </summary>
        public FoundationType FoundationType { get; set; }

        /// <summary>
        /// Represents the <see cref="Year"/> in which this building was built.
        /// </summary>
        public Year ConstructionYear { get; set; }

        /// <summary>
        /// Represents the height of this building.
        /// </summary>
        public double BuildingHeight { get; set; }
    }
}
