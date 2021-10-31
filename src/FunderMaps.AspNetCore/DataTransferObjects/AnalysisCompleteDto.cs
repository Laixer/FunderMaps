using FunderMaps.Core.Types;
using System;

namespace FunderMaps.AspNetCore.DataTransferObjects;

/// <summary>
///     Represents a response model for the complete endpoint.
/// </summary>
public record AnalysisCompleteDto : AnalysisDto
{
    /// <summary>
    ///     Represents the year in which this building was built.
    /// </summary>
    public DateTimeOffset ConstructionYear { get; set; }

    /// <summary>
    ///     Represents the height of this building.
    /// </summary>
    public double? BuildingHeight { get; set; }

    /// <summary>
    ///     Ground level.
    /// </summary>
    public double? GroundLevel { get; set; }

    /// <summary>
    ///     Building surface area in m^2.
    /// </summary>
    public double? SurfaceArea { get; set; }

    /// <summary>
    ///     Foundation type.
    /// </summary>
    public FoundationType? FoundationType { get; set; }

    /// <summary>
    ///     Foundation type reliability.
    /// </summary>
    public Reliability FoundationTypeReliability { get; set; }

    /// <summary>
    ///     Foundation recovery type.
    /// </summary>
    public RecoveryDocumentType? RecoveryType { get; set; }

    /// <summary>
    ///     Represents the period of drought (droogstand) for this building.
    /// </summary>
    public double? Drystand { get; set; }

    /// <summary>
    ///     Foundation type reliability.
    /// </summary>
    public Reliability DrystandReliability { get; set; }

    /// <summary>
    ///     Represents the foundation risk for this building.
    /// </summary>
    public FoundationRisk? DrystandRisk { get; set; }

    /// <summary>
    ///     Dewatering depth.
    /// </summary>
    public double? DewateringDepth { get; set; }

    /// <summary>
    ///     Dewatering depth reliability.
    /// </summary>
    public Reliability DewateringDepthReliability { get; set; }

    /// <summary>
    ///     Dewatering depth risk.
    /// </summary>
    public FoundationRisk? DewateringDepthRisk { get; set; }

    /// <summary>
    ///     Biological infection.
    /// </summary>
    public string BioInfection { get; set; }

    /// <summary>
    ///     Biological infection reliability.
    /// </summary>
    public Reliability BioInfectionReliability { get; set; }

    /// <summary>
    ///     Biological infection risk.
    /// </summary>
    public FoundationRisk? BioInfectionRisk { get; set; }

    /// <summary>
    ///     Represents the estimated restoration costs for this building.
    /// </summary>
    public double? RestorationCosts { get; set; }

    /// <summary>
    ///     Statistics.
    /// </summary>
    public StatisticsDto Statistics { get; set; }
}
