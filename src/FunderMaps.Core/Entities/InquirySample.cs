using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities.Report;
using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    // FUTURE: Inherit from parent child enforcing contract.
    // FUTURE: Split into partial classes.

    /// <summary>
    ///     Inquiry sample entity.
    /// </summary>
    public sealed class InquirySample : RecordControl<InquirySample, int>, IReportEntity<InquirySample>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquirySample()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Inquiry identifier.
        /// </summary>
        public int Inquiry { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Required, Geocoder]
        public string Address { get; set; }

        /// <summary>
        ///     Note.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        /// <summary>
        ///     Base measurement level.
        /// </summary>
        [Required]
        public BaseMeasurementLevel BaseMeasurementLevel { get; set; } = BaseMeasurementLevel.NAP;

        // TODO: Constraint, see #203
        /// <summary>
        ///     Built year.
        /// </summary>
        public DateTime BuiltYear { get; set; }

        /// <summary>
        ///     Substructure.
        /// </summary>
        public Substructure Substructure { get; set; }

        //
        // Surrounding
        //

        /// <summary>
        ///     CPT.
        /// </summary>
        public string Cpt { get; set; }

        /// <summary>
        ///     Monitoring well.
        /// </summary>
        public string MonitoringWell { get; set; }

        /// <summary>
        ///     Groundwater level temp.
        /// </summary>
        public decimal GroundwaterLevelTemp { get; set; }

        /// <summary>
        ///     Ground level.
        /// </summary>
        public decimal GroundLevel { get; set; }

        /// <summary>
        ///     Groundwater level net.
        /// </summary>
        public decimal GroundwaterLevelNet { get; set; }

        //
        // Foundation
        //

        /// <summary>
        ///     Foundation type.
        /// </summary>
        public FoundationType Type { get; set; }

        /// <summary>
        ///     Enforcement term.
        /// </summary>
        public EnforcementTerm EnforcementTerm { get; set; }

        /// <summary>
        ///     Recovery advised.
        /// </summary>
        public bool RecoveryAdvised { get; set; }

        /// <summary>
        ///     Damage cause.
        /// </summary>
        public FoundationDamageCause DamageCause { get; set; }

        /// <summary>
        ///     Damage cause.
        /// </summary>
        public FoundationDamageCharacteristics DamageCharacteristics { get; set; }

        /// <summary>
        ///     Construction pile.
        /// </summary>
        public ConstructionPile ConstructionPile { get; set; }

        /// <summary>
        ///     Wood type.
        /// </summary>
        public WoodType WoodType { get; set; }

        /// <summary>
        ///     Wood encroachement.
        /// </summary>
        public WoodEncroachement WoodEncroachement { get; set; }

        //
        // Foundation Measurement
        //

        /// <summary>
        ///     Construction level.
        /// </summary>
        public decimal ConstructionLevel { get; set; }

        /// <summary>
        ///     Wood level.
        /// </summary>
        public decimal WoodLevel { get; set; }

        /// <summary>
        ///     Pile diameter top.
        /// </summary>
        public decimal PileDiameterTop { get; set; }

        /// <summary>
        ///     Pile diameter bottom.
        /// </summary>
        public decimal PileDiameterBottom { get; set; }

        /// <summary>
        ///     Pile head level.
        /// </summary>
        public decimal PileHeadLevel { get; set; }

        /// <summary>
        ///     Pile tip level.
        /// </summary>
        public decimal PileTipLevel { get; set; }

        /// <summary>
        ///     Foundation depth.
        /// </summary>
        public decimal FoundationDepth { get; set; }

        /// <summary>
        ///     Mason level.
        /// </summary>
        public decimal MasonLevel { get; set; }

        /// <summary>
        ///     Concrete charger length.
        /// </summary>
        public decimal ConcreteChargerLength { get; set; }

        /// <summary>
        ///     Pile distance length.
        /// </summary>
        public decimal PileDistanceLength { get; set; }

        /// <summary>
        ///     Wood penetration depth.
        /// </summary>
        public decimal WoodPenetrationDepth { get; set; }

        //
        // Foundation Assessment
        //

        /// <summary>
        ///     Overall quality.
        /// </summary>
        public FoundationQuality OverallQuality { get; set; }

        /// <summary>
        ///     Wood quality.
        /// </summary>
        public WoodQuality WoodQuality { get; set; }

        /// <summary>
        ///     Construction quality.
        /// </summary>
        public Quality ConstructionQuality { get; set; }

        /// <summary>
        ///     Wood capacity horizontal quality.
        /// </summary>
        public Quality WoodCapacityHorizontalQuality { get; set; }

        /// <summary>
        ///     Pile wood capacity vertical quality.
        /// </summary>
        public Quality PileWoodCapacityVerticalQuality { get; set; }

        /// <summary>
        ///     Carrying capacity quality.
        /// </summary>
        public Quality CarryingCapacityQuality { get; set; }

        /// <summary>
        ///     Mason quality.
        /// </summary>
        public Quality MasonQuality { get; set; }

        /// <summary>
        ///     Wood quality necessity.
        /// </summary>
        public bool WoodQualityNecessity { get; set; }

        //
        // Building
        //

        /// <summary>
        ///     Crack indoor restored.
        /// </summary>
        public bool CrackIndoorRestored { get; set; }

        /// <summary>
        ///     Crack indoor type.
        /// </summary>
        public CrackType CrackIndoorType { get; set; }

        /// <summary>
        ///     Crack indoor size.
        /// </summary>
        public decimal CrackIndoorSize { get; set; }

        /// <summary>
        ///     Crack facade front size.
        /// </summary>
        public bool CrackFacadeFrontRestored { get; set; }

        /// <summary>
        ///     Crack facade front type.
        /// </summary>
        public CrackType CrackFacadeFrontType { get; set; }

        /// <summary>
        ///     Crack facade front size.
        /// </summary>
        public decimal CrackFacadeFrontSize { get; set; }

        /// <summary>
        ///     Crack facade back size.
        /// </summary>
        public bool CrackFacadeBackRestored { get; set; }

        /// <summary>
        ///     Crack facade back type.
        /// </summary>
        public CrackType CrackFacadeBackType { get; set; }

        /// <summary>
        ///     Crack facade back size.
        /// </summary>
        public decimal CrackFacadeBackSize { get; set; }

        /// <summary>
        ///     Crack facade left size.
        /// </summary>
        public bool CrackFacadeLeftRestored { get; set; }

        /// <summary>
        ///     Crack facade left type.
        /// </summary>
        public CrackType CrackFacadeLeftType { get; set; }

        /// <summary>
        ///     Crack facade left size.
        /// </summary>
        public decimal CrackFacadeLeftSize { get; set; }

        /// <summary>
        ///     Crack facade right size.
        /// </summary>
        public bool CrackFacadeRightRestored { get; set; }

        /// <summary>
        ///     Crack facade right type.
        /// </summary>
        public CrackType CrackFacadeRightType { get; set; }

        /// <summary>
        ///     Crack facade right size.
        /// </summary>
        public decimal CrackFacadeRightSize { get; set; }

        /// <summary>
        ///     Deformed facade.
        /// </summary>
        public bool DeformedFacade { get; set; }

        /// <summary>
        ///     Threshold updown skewed.
        /// </summary>
        public bool ThresholdUpdownSkewed { get; set; }

        /// <summary>
        ///     Threshold front level.
        /// </summary>
        public decimal ThresholdFrontLevel { get; set; }

        /// <summary>
        ///     Threshold back level.
        /// </summary>
        public decimal ThresholdBackLevel { get; set; }

        /// <summary>
        ///     Skewed parallel.
        /// </summary>
        public decimal SkewedParallel { get; set; }

        /// <summary>
        ///     Skewed perpendicular.
        /// </summary>
        public decimal SkewedPerpendicular { get; set; }

        /// <summary>
        ///     Skewed facade.
        /// </summary>
        public RotationType SkewedFacade { get; set; }

        /// <summary>
        ///     Settlement speed.
        /// </summary>
        public decimal SettlementSpeed { get; set; }

        public override void InitializeDefaults()
        {
            Id = 0;
            BaseMeasurementLevel = BaseMeasurementLevel.NAP;
            CreateDate = DateTime.MinValue;
            UpdateDate = null;
            DeleteDate = null;
        }
    }
}
