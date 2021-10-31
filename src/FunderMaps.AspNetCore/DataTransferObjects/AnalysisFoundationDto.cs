using FunderMaps.Core.Types;

namespace FunderMaps.AspNetCore.DataTransferObjects;

/// <summary>
///     Represents a response model for the foundation endpoint.
/// </summary>
public record AnalysisFoundationDto : AnalysisDto
{
    /// <summary>
    ///     Ground level.
    /// </summary>
    public double? GroundLevel { get; set; }

    /// <summary>
    ///     Foundation type.
    /// </summary>
    public FoundationType? FoundationType { get; set; }

    /// <summary>
    ///     Foundation type reliability.
    /// </summary>
    public Reliability FoundationTypeReliability { get; set; }

    /// <summary>
    ///     Soil code.
    /// </summary>
    public string Soil { get; set; }

    /// <summary>
    ///     Represents the ground water level.
    /// </summary>
    public double? GroundWaterLevel { get; set; }

    /// <summary>
    ///     Represents the foundation risk for this building.
    /// </summary>
    public FoundationRisk? DrystandRisk { get; set; }

    /// <summary>
    ///     Dewatering depth risk.
    /// </summary>
    public FoundationRisk? DewateringDepthRisk { get; set; }

    /// <summary>
    ///     Biological infection risk.
    /// </summary>
    public FoundationRisk? BioInfectionRisk { get; set; }
}
