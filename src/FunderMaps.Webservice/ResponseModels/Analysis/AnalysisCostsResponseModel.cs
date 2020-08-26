﻿using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels.Analysis
{
    /// <summary>
    /// Represents a response model for the costs endpoint.
    /// </summary>
    public sealed class AnalysisCostsResponseModel : AnalysisResponseModelBase
    {
        /// <summary>
        /// Represents the foundation risk for this building.
        /// </summary>
        public FoundationRiskResponseModel FoundationRisk { get; set; }

        /// <summary>
        /// Represents the estimated restoration costs for this building.
        /// </summary>
        public double? RestorationCosts { get; set; }

        /// <summary>
        /// Represents the reliability of all data about this building.
        /// </summary>
        public ReliabilityResponseModel? Reliability { get; set; }

        /// <summary>
        /// Total amount of restored buildings in the given area.
        /// </summary>
        public uint? TotalBuildingRestoredCount { get; set; }

        /// <summary>
        /// Total amount of incidents in the given region.
        /// </summary>
        public uint? TotalIncidentCount { get; set; }
    }
}
