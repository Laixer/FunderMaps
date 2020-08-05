using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;

namespace FunderMaps.Core.Types.Products
{
    /// <summary>
    /// Represents a model for the complete endpoint.
    /// </summary>
    public sealed class AnalysisProduct : ProductBase
    {
        /// <summary>
        /// Represents the internal id of this building.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Represents the external id of this building.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Represents the external data source of this building.
        /// </summary>
        public ExternalDataSource? ExternalSource { get; set; }

        /// <summary>
        /// Represents the foundation type of this building.
        /// </summary>
        public FoundationType? FoundationType { get; set; }

        /// <summary>
        /// Represents the ground water level.
        /// TODO Unit and reference?
        /// </summary>
        public double? GroundWaterLevel { get; set; }

        /// <summary>
        /// Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk? FoundationRisk { get; set; }

        /// <summary>
        /// Represents the <see cref="Year"/> in which this building was built.
        /// </summary>
        public Year ConstructionYear { get; set; }

        /// <summary>
        /// Represents the height of this building.
        /// </summary>
        public double? BuildingHeight { get; set; }

        /// <summary>
        /// Description of the terrain on which this building lies.
        /// TODO Correct name?
        /// </summary>
        public string TerrainDescription { get; set; }

        /// <summary>
        /// Represents the ground level (maaiveldniveau) of this building.
        /// </summary>
        public double? GroundLevel { get; set; }

        /// <summary>
        /// Represents the estimated restoration costs for this building.
        /// TODO Correct unit?
        /// </summary>
        public double? RestorationCosts { get; set; }

        /// <summary>
        /// Represents the dewatering depth (ontwateringsdiepte) for this building.
        /// TODO Correct unit?
        /// TODO Correct name?
        /// </summary>
        public double? DewateringDepth { get; set; }

        /// <summary>
        /// Represents the period of drought (droogstand) for this building.
        /// TODO Correct unit?
        /// TODO Correct name?
        /// </summary>
        public double? DryPeriod { get; set; }

        /// <summary>
        /// Complete description of this building.
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// Represents the reliability of all data about this building.
        /// TODO What unit? Percentage?
        /// TODO Correct name?
        /// </summary>
        public double? Reliability { get; set; }

        /// <summary>
        /// Represents the distribution of foundation types.
        /// </summary>
        public FoundationTypeDistribution FoundationTypeDistribution { get; set; }

        /// <summary>
        /// Represents the distribution of building construction years in the
        /// given region.
        /// </summary>
        public ConstructionYearDistribution ConstructionYearDistribution { get; set; }

        /// <summary>
        /// Represents the distribution of foundation risks in the given region.
        /// </summary>
        public FoundationRiskDistribution FoundationRiskDistribution { get; set; }

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
