using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Inquiry sample entity.
/// </summary>
public sealed class InquirySample : RecordControl, IEntityIdentifier<int>
{
    public int Identifier => Id;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Inquiry identifier.
    /// </summary>
    public int Inquiry { get; set; }

    /// <summary>
    ///     An address identifier.
    /// </summary>
    [Required]
    public string Address { get; set; } = default!;

    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Required]
    public string Building { get; set; } = default!;

    /// <summary>
    ///     Note.
    /// </summary>
    [DataType(DataType.MultilineText)]
    public string? Note { get; set; }

    /// <summary>
    ///     Built year.
    /// </summary>
    public DateTime? BuiltYear { get; set; }

    /// <summary>
    ///     Substructure.
    /// </summary>
    public Substructure? Substructure { get; set; }

    //
    // Surrounding
    //

    /// <summary>
    ///     CPT.
    /// </summary>
    public string? Cpt { get; set; }

    /// <summary>
    ///     Monitoring well.
    /// </summary>
    public string? MonitoringWell { get; set; }

    /// <summary>
    ///     Groundwater level temp.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? GroundwaterLevelTemp { get; set; }

    /// <summary>
    ///     Ground level.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? GroundLevel { get; set; }

    /// <summary>
    ///     Groundwater level net.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? GroundwaterLevelNet { get; set; }

    //
    // Foundation
    //

    /// <summary>
    ///     Foundation type.
    /// </summary>
    public FoundationType? FoundationType { get; set; }

    /// <summary>
    ///     Enforcement term.
    /// </summary>
    public EnforcementTerm? EnforcementTerm { get; set; }

    /// <summary>
    ///     Recovery advised.
    /// </summary>
    public bool? RecoveryAdvised { get; set; }

    /// <summary>
    ///     Damage cause.
    /// </summary>
    public FoundationDamageCause? DamageCause { get; set; }

    /// <summary>
    ///     Damage cause.
    /// </summary>
    public FoundationDamageCharacteristics? DamageCharacteristics { get; set; }

    /// <summary>
    ///     Construction pile.
    /// </summary>
    public ConstructionPile? ConstructionPile { get; set; }

    /// <summary>
    ///     Wood type.
    /// </summary>
    public WoodType? WoodType { get; set; }

    /// <summary>
    ///     Wood encroachement.
    /// </summary>
    public WoodEncroachement? WoodEncroachement { get; set; }

    //
    // Foundation Measurement
    //

    /// <summary>
    ///     Construction level.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? ConstructionLevel { get; set; }

    /// <summary>
    ///     Wood level.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? WoodLevel { get; set; }

    /// <summary>
    ///     Pile diameter top.
    /// </summary>
    [Range(0.0, 999.99)]
    public decimal? PileDiameterTop { get; set; }

    /// <summary>
    ///     Pile diameter bottom.
    /// </summary>
    [Range(0.0, 999.99)]
    public decimal? PileDiameterBottom { get; set; }

    /// <summary>
    ///     Pile head level.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? PileHeadLevel { get; set; }

    /// <summary>
    ///     Pile tip level.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? PileTipLevel { get; set; }

    /// <summary>
    ///     Foundation depth.
    /// </summary>
    [Range(0.0, 999.99)]
    public decimal? FoundationDepth { get; set; }

    /// <summary>
    ///     Mason level.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? MasonLevel { get; set; }

    /// <summary>
    ///     Concrete charger length.
    /// </summary>
    [Range(0.0, 999.99)]
    public decimal? ConcreteChargerLength { get; set; }

    /// <summary>
    ///     Pile distance length.
    /// </summary>
    [Range(0.0, 999.99)]
    public decimal? PileDistanceLength { get; set; }

    /// <summary>
    ///     Wood penetration depth.
    /// </summary>
    [Range(0.0, 999.99)]
    public decimal? WoodPenetrationDepth { get; set; }

    //
    // Foundation Assessment
    //

    /// <summary>
    ///     Overall quality.
    /// </summary>
    public FoundationQuality? OverallQuality { get; set; }

    /// <summary>
    ///     Wood quality.
    /// </summary>
    public WoodQuality? WoodQuality { get; set; }

    /// <summary>
    ///     Construction quality.
    /// </summary>
    public Quality? ConstructionQuality { get; set; }

    /// <summary>
    ///     Wood capacity horizontal quality.
    /// </summary>
    public Quality? WoodCapacityHorizontalQuality { get; set; }

    /// <summary>
    ///     Pile wood capacity vertical quality.
    /// </summary>
    public Quality? PileWoodCapacityVerticalQuality { get; set; }

    /// <summary>
    ///     Carrying capacity quality.
    /// </summary>
    public Quality? CarryingCapacityQuality { get; set; }

    /// <summary>
    ///     Mason quality.
    /// </summary>
    public Quality? MasonQuality { get; set; }

    /// <summary>
    ///     Wood quality necessity.
    /// </summary>
    public bool? WoodQualityNecessity { get; set; }

    //
    // Building
    //

    /// <summary>
    ///     Crack indoor restored.
    /// </summary>
    public bool? CrackIndoorRestored { get; set; }

    /// <summary>
    ///     Crack indoor type.
    /// </summary>
    public CrackType? CrackIndoorType { get; set; }

    /// <summary>
    ///     Crack indoor size.
    /// </summary>
    [Range(0, 999)]
    public int? CrackIndoorSize { get; set; }

    /// <summary>
    ///     Crack facade front size.
    /// </summary>
    public bool? CrackFacadeFrontRestored { get; set; }

    /// <summary>
    ///     Crack facade front type.
    /// </summary>
    public CrackType? CrackFacadeFrontType { get; set; }

    /// <summary>
    ///     Crack facade front size.
    /// </summary>
    [Range(0, 999)]
    public int? CrackFacadeFrontSize { get; set; }

    /// <summary>
    ///     Crack facade back size.
    /// </summary>
    public bool? CrackFacadeBackRestored { get; set; }

    /// <summary>
    ///     Crack facade back type.
    /// </summary>
    public CrackType? CrackFacadeBackType { get; set; }

    /// <summary>
    ///     Crack facade back size.
    /// </summary>
    [Range(0, 999)]
    public int? CrackFacadeBackSize { get; set; }

    /// <summary>
    ///     Crack facade left size.
    /// </summary>
    public bool? CrackFacadeLeftRestored { get; set; }

    /// <summary>
    ///     Crack facade left type.
    /// </summary>
    public CrackType? CrackFacadeLeftType { get; set; }

    /// <summary>
    ///     Crack facade left size.
    /// </summary>
    [Range(0, 999)]
    public int? CrackFacadeLeftSize { get; set; }

    /// <summary>
    ///     Crack facade right size.
    /// </summary>
    public bool? CrackFacadeRightRestored { get; set; }

    /// <summary>
    ///     Crack facade right type.
    /// </summary>
    public CrackType? CrackFacadeRightType { get; set; }

    /// <summary>
    ///     Crack facade right size.
    /// </summary>
    [Range(0, 999)]
    public int? CrackFacadeRightSize { get; set; }

    /// <summary>
    ///     Deformed facade.
    /// </summary>
    public bool? DeformedFacade { get; set; }

    /// <summary>
    ///     Threshold updown skewed.
    /// </summary>
    public bool? ThresholdUpdownSkewed { get; set; }

    /// <summary>
    ///     Threshold front level.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? ThresholdFrontLevel { get; set; }

    /// <summary>
    ///     Threshold back level.
    /// </summary>
    [Range(-999.99, 999.99)]
    public decimal? ThresholdBackLevel { get; set; }

    /// <summary>
    ///     Skewed parallel.
    /// </summary>
    [Range(0.0, 999.99)]
    public decimal? SkewedParallel { get; set; }

    /// <summary>
    ///     Skewed parallel facade.
    /// </summary>
    public RotationType? SkewedParallelFacade { get; set; }

    /// <summary>
    ///     Skewed perpendicular.
    /// </summary>
    [Range(0.0, 999.99)]
    public decimal? SkewedPerpendicular { get; set; }

    /// <summary>
    ///     Skewed perpendicular facade.
    /// </summary>
    public RotationType? SkewedPerpendicularFacade { get; set; }

    /// <summary>
    ///     Settlement speed.
    /// </summary>
    public double? SettlementSpeed { get; set; }

    /// <summary>
    ///     Skewed window and/or frame.
    /// </summary>
    public bool? SkewedWindowFrame { get; set; }
}
