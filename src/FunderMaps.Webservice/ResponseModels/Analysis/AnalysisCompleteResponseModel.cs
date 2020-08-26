using FunderMaps.Webservice.ResponseModels.Types;
using System;

namespace FunderMaps.Webservice.ResponseModels.Analysis
{
    /// <summary>
    /// Represents a response model for the complete endpoint.
    /// </summary>
    public sealed class AnalysisCompleteResponseModel : AnalysisResponseModelBase
    {
        /// <summary>
        /// Represents the foundation type of this building.
        /// </summary>
        public FoundationTypeResponseModel FoundationType { get; set; }

        /// <summary>
        /// Represents the ground water level.
        /// </summary>
        public double? GroundWaterLevel { get; set; }

        /// <summary>
        /// Represents the foundation risk for this building.
        /// </summary>
        public FoundationRiskResponseModel FoundationRisk { get; set; }

        /// <summary>
        /// Represents the <see cref="Year"/> in which this building was built.
        /// </summary>
        public DateTimeOffset ConstructionYear { get; set; }

        /// <summary>
        /// Represents the height of this building.
        /// </summary>
        public double? BuildingHeight { get; set; }

        /// <summary>
        /// Description of the terrain on which this building lies.
        /// </summary>
        public string TerrainDescription { get; set; }

        /// <summary>
        /// Represents the ground level (maaiveldniveau) of this building.
        /// </summary>
        public double? GroundLevel { get; set; }

        /// <summary>
        /// Represents the estimated restoration costs for this building.
        /// </summary>
        public double? RestorationCosts { get; set; }

        /// <summary>
        /// Represents the dewatering depth (ontwateringsdiepte) for this building.
        /// </summary>
        public double? DewateringDepth { get; set; }

        /// <summary>
        /// Represents drystand (droogstand) for this building.
        /// </summary>
        public double? Drystand { get; set; }

        /// <summary>
        /// Complete description of this building.
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// Represents the reliability of all data about this building.
        /// </summary>
        public ReliabilityResponseModel? Reliability { get; set; }

        /// <summary>
        /// Represents the distribution of foundation types.
        /// </summary>
        public FoundationTypeDistributionResponseModel FoundationTypeDistribution { get; set; }

        /// <summary>
        /// Represents the distribution of building construction years in the
        /// given region.
        /// </summary>
        public ConstructionYearDistributionResponseModel ConstructionYearDistribution { get; set; }

        /// <summary>
        /// Represents the distribution of foundation risks in the given region.
        /// </summary>
        public FoundationRiskDistributionResponseModel FoundationRiskDistribution { get; set; }

        /// <summary>
        /// Represents the percentage of collected data in the given region.
        /// </summary>
        public double? DataCollectedPercentage { get; set; }

        /// <summary>
        /// Total amount of restored buildings in the given area.
        /// </summary>
        public uint? TotalBuildingRestoredCount { get; set; }

        /// <summary>
        /// Total amount of incidents in the given region.
        /// </summary>
        public uint? TotalIncidentCount { get; set; }

        /// <summary>
        /// Total amount of reports in the given region.
        /// </summary>
        public uint? TotalReportCount { get; set; }
    }
}
