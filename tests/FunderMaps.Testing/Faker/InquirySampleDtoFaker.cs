using Bogus;
using Bogus.Extensions;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="InquirySampleDto"/>.
    /// </summary>
    public class InquirySampleDtoFaker : Faker<InquirySampleDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquirySampleDtoFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Inquiry, f => f.UniqueIndex);
            RuleFor(f => f.Address, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.Note, f => f.Lorem.Text().OrNull(f, .5f));
            RuleFor(f => f.BuiltYear, f => f.Date.Recent().OrNull(f, .7f));
            RuleFor(f => f.Substructure, f => f.PickRandom<Substructure>().OrNull(f, .5f));
            RuleFor(f => f.Cpt, f => f.Commerce.Product().OrNull(f, .5f));
            RuleFor(f => f.MonitoringWell, f => f.Commerce.Product().OrNull(f, .5f));
            RuleFor(f => f.GroundwaterLevelTemp, f => f.Random.Decimal(-100, 200).OrNull(f, .5f));
            RuleFor(f => f.GroundLevel, f => f.Random.Decimal(-300, 500).OrNull(f, .5f));
            RuleFor(f => f.GroundwaterLevelNet, f => f.Random.Decimal(-100, 200).OrNull(f, .5f));
            RuleFor(f => f.FoundationType, f => f.PickRandom<FoundationType>().OrNull(f, .5f));
            RuleFor(f => f.EnforcementTerm, f => f.PickRandom<EnforcementTerm>().OrNull(f, .5f));
            RuleFor(f => f.RecoveryAdvised, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.DamageCause, f => f.PickRandom<FoundationDamageCause>().OrNull(f, .5f));
            RuleFor(f => f.DamageCharacteristics, f => f.PickRandom<FoundationDamageCharacteristics>().OrNull(f, .5f));
            RuleFor(f => f.ConstructionPile, f => f.PickRandom<ConstructionPile>().OrNull(f, .5f));
            RuleFor(f => f.WoodType, f => f.PickRandom<WoodType>().OrNull(f, .5f));
            RuleFor(f => f.WoodEncroachement, f => f.PickRandom<WoodEncroachement>().OrNull(f, .5f));
            RuleFor(f => f.ConstructionLevel, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.WoodLevel, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.PileDiameterTop, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.PileDiameterBottom, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.PileHeadLevel, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.PileTipLevel, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.FoundationDepth, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.MasonLevel, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.ConcreteChargerLength, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.PileDistanceLength, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.WoodPenetrationDepth, f => f.Random.Decimal(0, 30).OrNull(f, .5f));
            RuleFor(f => f.OverallQuality, f => f.PickRandom<FoundationQuality>().OrNull(f, .5f));
            RuleFor(f => f.WoodQuality, f => f.PickRandom<WoodQuality>().OrNull(f, .5f));
            RuleFor(f => f.ConstructionQuality, f => f.PickRandom<Quality>().OrNull(f, .5f));
            RuleFor(f => f.WoodCapacityHorizontalQuality, f => f.PickRandom<Quality>().OrNull(f, .5f));
            RuleFor(f => f.PileWoodCapacityVerticalQuality, f => f.PickRandom<Quality>().OrNull(f, .5f));
            RuleFor(f => f.CarryingCapacityQuality, f => f.PickRandom<Quality>().OrNull(f, .5f));
            RuleFor(f => f.MasonQuality, f => f.PickRandom<Quality>().OrNull(f, .5f));
            RuleFor(f => f.WoodQualityNecessity, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.CrackIndoorRestored, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.CrackIndoorType, f => f.PickRandom<CrackType>().OrNull(f, .5f));
            RuleFor(f => f.CrackIndoorSize, f => f.Random.Decimal(0, 10).OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeFrontRestored, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeFrontType, f => f.PickRandom<CrackType>().OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeFrontSize, f => f.Random.Decimal(0, 10).OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeBackRestored, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeBackType, f => f.PickRandom<CrackType>().OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeBackSize, f => f.Random.Decimal(0, 10).OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeLeftRestored, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeLeftType, f => f.PickRandom<CrackType>().OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeLeftSize, f => f.Random.Decimal(0, 10).OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeRightRestored, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeRightType, f => f.PickRandom<CrackType>().OrNull(f, .5f));
            RuleFor(f => f.CrackFacadeRightSize, f => f.Random.Decimal(0, 10).OrNull(f, .5f));
            RuleFor(f => f.DeformedFacade, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.ThresholdUpdownSkewed, f => f.Random.Bool().OrNull(f, .5f));
            RuleFor(f => f.ThresholdFrontLevel, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.ThresholdBackLevel, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.SkewedParallel, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.SkewedPerpendicular, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.SkewedFacade, f => f.PickRandom<RotationType>().OrNull(f, .5f));
            RuleFor(f => f.SettlementSpeed, f => f.Random.Decimal(-50, 50).OrNull(f, .5f));
            RuleFor(f => f.SkewedWindowFrame, f => f.Random.Bool().OrNull(f, .5f));
        }
    }
}
