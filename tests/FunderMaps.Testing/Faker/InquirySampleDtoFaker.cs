using Bogus;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="InquirySampleDto"/>.
    /// </summary>
    public class InquirySampleDtoFaker : Faker<InquirySampleDto>
    {
        public InquirySampleDtoFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Inquiry, f => f.UniqueIndex);
            RuleFor(f => f.Address, f => $"gfm-{f.Random.Hash(32)}");
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.BuiltYear, f => f.Date.Recent());
            RuleFor(f => f.Substructure, f => f.PickRandom<Substructure>());
            RuleFor(f => f.Cpt, f => f.Commerce.Product());
            RuleFor(f => f.MonitoringWell, f => f.Commerce.Product());
            RuleFor(f => f.GroundwaterLevelTemp, f => f.Random.Decimal(-100, 200));
            RuleFor(f => f.GroundLevel, f => f.Random.Decimal(-300, 500));
            RuleFor(f => f.GroundwaterLevelNet, f => f.Random.Decimal(-100, 200));
            RuleFor(f => f.FoundationType, f => f.PickRandom<FoundationType>());
            RuleFor(f => f.EnforcementTerm, f => f.PickRandom<EnforcementTerm>());
            RuleFor(f => f.RecoveryAdvised, f => f.Random.Bool());
            RuleFor(f => f.DamageCause, f => f.PickRandom<FoundationDamageCause>());
            RuleFor(f => f.DamageCharacteristics, f => f.PickRandom<FoundationDamageCharacteristics>());
            RuleFor(f => f.ConstructionPile, f => f.PickRandom<ConstructionPile>());
            RuleFor(f => f.WoodType, f => f.PickRandom<WoodType>());
            RuleFor(f => f.WoodEncroachement, f => f.PickRandom<WoodEncroachement>());
            RuleFor(f => f.ConstructionLevel, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.WoodLevel, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.PileDiameterTop, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.PileDiameterBottom, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.PileHeadLevel, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.PileTipLevel, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.FoundationDepth, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.MasonLevel, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.ConcreteChargerLength, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.PileDistanceLength, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.WoodPenetrationDepth, f => f.Random.Decimal(0, 30));
            RuleFor(f => f.OverallQuality, f => f.PickRandom<FoundationQuality>());
            RuleFor(f => f.WoodQuality, f => f.PickRandom<WoodQuality>());
            RuleFor(f => f.ConstructionQuality, f => f.PickRandom<Quality>());
            RuleFor(f => f.WoodCapacityHorizontalQuality, f => f.PickRandom<Quality>());
            RuleFor(f => f.PileWoodCapacityVerticalQuality, f => f.PickRandom<Quality>());
            RuleFor(f => f.CarryingCapacityQuality, f => f.PickRandom<Quality>());
            RuleFor(f => f.MasonQuality, f => f.PickRandom<Quality>());
            RuleFor(f => f.WoodQualityNecessity, f => f.Random.Bool());
            RuleFor(f => f.CrackIndoorRestored, f => f.Random.Bool());
            RuleFor(f => f.CrackIndoorType, f => f.PickRandom<CrackType>());
            RuleFor(f => f.CrackIndoorSize, f => f.Random.Decimal(0, 10));
            RuleFor(f => f.CrackFacadeFrontRestored, f => f.Random.Bool());
            RuleFor(f => f.CrackFacadeFrontType, f => f.PickRandom<CrackType>());
            RuleFor(f => f.CrackFacadeFrontSize, f => f.Random.Decimal(0, 10));
            RuleFor(f => f.CrackFacadeBackRestored, f => f.Random.Bool());
            RuleFor(f => f.CrackFacadeBackType, f => f.PickRandom<CrackType>());
            RuleFor(f => f.CrackFacadeBackSize, f => f.Random.Decimal(0, 10));
            RuleFor(f => f.CrackFacadeLeftRestored, f => f.Random.Bool());
            RuleFor(f => f.CrackFacadeLeftType, f => f.PickRandom<CrackType>());
            RuleFor(f => f.CrackFacadeLeftSize, f => f.Random.Decimal(0, 10));
            RuleFor(f => f.CrackFacadeRightRestored, f => f.Random.Bool());
            RuleFor(f => f.CrackFacadeRightType, f => f.PickRandom<CrackType>());
            RuleFor(f => f.CrackFacadeRightSize, f => f.Random.Decimal(0, 10));
            RuleFor(f => f.DeformedFacade, f => f.Random.Bool());
            RuleFor(f => f.ThresholdUpdownSkewed, f => f.Random.Bool());
            RuleFor(f => f.ThresholdFrontLevel, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.ThresholdBackLevel, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.SkewedParallel, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.SkewedPerpendicular, f => f.Random.Decimal(-50, 50));
            RuleFor(f => f.SkewedFacade, f => f.PickRandom<RotationType>());
            RuleFor(f => f.SettlementSpeed, f => f.Random.Decimal(-50, 50));
        }
    }
}
