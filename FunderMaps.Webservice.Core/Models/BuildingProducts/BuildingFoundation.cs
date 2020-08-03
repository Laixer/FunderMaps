﻿using FunderMaps.Webservice.Enums;

namespace FunderMaps.Webservice.Core.Models.Building
{
    /// <summary>
    /// Represents a model for the foundation endpoint.
    /// </summary>
    public class BuildingFoundation : BuildingBase
    {
        /// <summary>
        /// Represents the foundation type of this building.
        /// </summary>
        public FoundationType FoundationType { get; set; }

        /// <summary>
        /// Represents the ground water level.
        /// TODO Unit and reference?
        /// </summary>
        public double GroundWaterLevel { get; set; }

        /// <summary>
        /// Represents the foundation risk for this building.
        /// </summary>
        public FoundationRisk FoundationRisk { get; set; }

        /// <summary>
        /// Represents the ground level (maaiveldniveau) of this building.
        /// </summary>
        public double GroundLevel { get; set; }

        /// <summary>
        /// Represents the dewatering depth (ontwateringsdiepte) for this building.
        /// TODO Correct unit?
        /// TODO Correct name?
        /// </summary>
        public double DewateringDepth { get; set; }

        /// <summary>
        /// Represents the period of drought (droogstand) for this building.
        /// TODO Correct unit?
        /// TODO Correct name?
        /// </summary>
        public double DryPeriod { get; set; }
    }
}
