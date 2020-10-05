using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Webservice.ResponseModels.Analysis
{
    /// <summary>
    ///     Represents a response model for the building data endpoint.
    /// </summary>
    public sealed class AnalysisBuildingDataResponseModel : AnalysisResponseModelBase
    {
        /// <summary>
        ///     Represents the foundation type of this building.
        /// </summary>
        public FoundationType FoundationType { get; set; }

        /// <summary>
        ///     Represents the <see cref="Year"/> in which this building was built.
        /// </summary>
        public DateTimeOffset ConstructionYear { get; set; }

        /// <summary>
        ///     Represents the height of this building.
        /// </summary>
        public double? BuildingHeight { get; set; }
    }
}
