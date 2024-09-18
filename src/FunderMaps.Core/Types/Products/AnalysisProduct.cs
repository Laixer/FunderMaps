using System.ComponentModel.DataAnnotations;
using FunderMaps.Core.DataAnnotations;

namespace FunderMaps.Core.Types.Products;

/// <summary>
///     Represents a model for the complete endpoint.
/// </summary>
public sealed record AnalysisProduct
{
    /// <summary>
    ///     Building external identifier.
    /// </summary>
    [Required]
    public string BuildingId { get; init; } = default!;

    /// <summary>
    ///     Neighborhood identifier.
    /// </summary>
    [Required, Geocoder]
    public string? NeighborhoodId { get; init; } = default!;

    /// <summary>
    ///     Built year.
    /// </summary>
    public int? ConstructionYear { get; init; }

    /// <summary>
    ///     Construction year reliability.
    /// </summary>
    public Reliability ConstructionYearReliability { get; init; }

    /// <summary>
    ///     Foundation recovery type.
    /// </summary>
    public RecoveryType? RecoveryType { get; init; }

    /// <summary>
    ///     Represents the estimated restoration costs for this building.
    /// </summary>
    public int? RestorationCosts { get; init; }

    /// <summary>
    ///     Represents the height of this building.
    /// </summary>
    public double? Height { get; init; }

    /// <summary>
    ///     Building subsidence velocity.
    /// </summary>
    public double? Velocity { get; init; }

    /// <summary>
    ///     Represents the ground water level.
    /// </summary>
    public double? GroundWaterLevel { get; init; }

    /// <summary>
    ///     Ground level in meters.
    /// </summary>
    public double? GroundLevel { get; init; }

    /// <summary>
    ///     Soil code.
    /// </summary>
    public string? Soil { get; init; }

    /// <summary>
    ///     Building surface area in square meters.
    /// </summary>
    public double? SurfaceArea { get; init; }

    /// <summary>
    ///     Damage cause.
    /// </summary>
    public FoundationDamageCause? DamageCause { get; init; }

    /// <summary>
    ///     Enforcement term.
    /// </summary>
    public EnforcementTerm? EnforcementTerm { get; init; }

    /// <summary>
    ///     Enforcement term.
    /// </summary>
    public FoundationQuality? OverallQuality { get; init; }

    /// <summary>
    ///     Report type.
    /// </summary>
    public InquiryType? InquiryType { get; init; }

    /// <summary>
    ///     Foundation type.
    /// </summary>
    public FoundationType? FoundationType { get; init; }

    /// <summary>
    ///     Foundation type reliability.
    /// </summary>
    public Reliability FoundationTypeReliability { get; init; }

    /// <summary>
    ///     Represents the period of drought (droogstand) for this building.
    /// </summary>
    public double? Drystand { get; init; }

    /// <summary>
    ///     Foundation type reliability.
    /// </summary>
    public Reliability DrystandReliability { get; init; }

    /// <summary>
    ///     Represents the foundation risk for this building.
    /// </summary>
    public FoundationRisk? DrystandRisk { get; init; }

    /// <summary>
    ///     Dewatering depth.
    /// </summary>
    public double? DewateringDepth { get; init; }

    /// <summary>
    ///     Dewatering depth reliability.
    /// </summary>
    public Reliability DewateringDepthReliability { get; init; }

    /// <summary>
    ///     Dewatering depth risk.
    /// </summary>
    public FoundationRisk? DewateringDepthRisk { get; init; }

    /// <summary>
    ///     Biological infection reliability.
    /// </summary>
    public Reliability BioInfectionReliability { get; init; }

    /// <summary>
    ///     Biological infection risk.
    /// </summary>
    public FoundationRisk? BioInfectionRisk { get; init; }

    /// <summary>
    ///     Unclassified risk.
    /// </summary>
    public FoundationRisk? UnclassifiedRisk { get; init; }
}
