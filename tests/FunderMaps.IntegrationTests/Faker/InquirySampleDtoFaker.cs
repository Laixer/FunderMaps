using Bogus;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.IntegrationTests.Faker
{
    public class InquirySampleDtoFaker : Faker<InquirySampleDto>
    {
        public InquirySampleDtoFaker()
        {
            RuleFor(f => f.Id, f => new InquirySampleFaker().Generate().Id);
            RuleFor(f => f.Inquiry, f => new InquiryFaker().Generate().Id);
            RuleFor(f => f.Address, f => new AddressFaker().Generate().Id);
            RuleFor(f => f.Note, f => new InquirySampleFaker().Generate().Note);
            RuleFor(f => f.BuiltYear, f => new InquirySampleFaker().Generate().BuiltYear);
            RuleFor(f => f.Substructure, f => new InquirySampleFaker().Generate().Substructure);
            RuleFor(f => f.Cpt, f => new InquirySampleFaker().Generate().Cpt);
            RuleFor(f => f.MonitoringWell, f => new InquirySampleFaker().Generate().MonitoringWell);
            RuleFor(f => f.GroundwaterLevelTemp, f => new InquirySampleFaker().Generate().GroundwaterLevelTemp);
            RuleFor(f => f.GroundLevel, f => new InquirySampleFaker().Generate().GroundLevel);
            RuleFor(f => f.GroundwaterLevelNet, f => new InquirySampleFaker().Generate().GroundwaterLevelNet);
            RuleFor(f => f.Type, f => new InquirySampleFaker().Generate().Type);
            RuleFor(f => f.EnforcementTerm, f => new InquirySampleFaker().Generate().EnforcementTerm);
            RuleFor(f => f.RecoveryAdvised, f => new InquirySampleFaker().Generate().RecoveryAdvised);
            RuleFor(f => f.DamageCause, f => new InquirySampleFaker().Generate().DamageCause);
            RuleFor(f => f.DamageCharacteristics, f => new InquirySampleFaker().Generate().DamageCharacteristics);
            RuleFor(f => f.ConstructionPile, f => new InquirySampleFaker().Generate().ConstructionPile);
            RuleFor(f => f.WoodType, f => new InquirySampleFaker().Generate().WoodType);
            RuleFor(f => f.WoodEncroachement, f => new InquirySampleFaker().Generate().WoodEncroachement);
            RuleFor(f => f.ConstructionLevel, f => new InquirySampleFaker().Generate().ConstructionLevel);
            RuleFor(f => f.WoodLevel, f => new InquirySampleFaker().Generate().WoodLevel);
            RuleFor(f => f.PileDiameterTop, f => new InquirySampleFaker().Generate().PileDiameterTop);
            RuleFor(f => f.PileDiameterBottom, f => new InquirySampleFaker().Generate().PileDiameterBottom);
            RuleFor(f => f.PileHeadLevel, f => new InquirySampleFaker().Generate().PileHeadLevel);
            RuleFor(f => f.PileTipLevel, f => new InquirySampleFaker().Generate().PileTipLevel);
            RuleFor(f => f.FoundationDepth, f => new InquirySampleFaker().Generate().FoundationDepth);
            RuleFor(f => f.MasonLevel, f => new InquirySampleFaker().Generate().MasonLevel);
            RuleFor(f => f.ConcreteChargerLength, f => new InquirySampleFaker().Generate().ConcreteChargerLength);
            RuleFor(f => f.PileDistanceLength, f => new InquirySampleFaker().Generate().PileDistanceLength);
            RuleFor(f => f.WoodPenetrationDepth, f => new InquirySampleFaker().Generate().WoodPenetrationDepth);
            RuleFor(f => f.OverallQuality, f => new InquirySampleFaker().Generate().OverallQuality);
            RuleFor(f => f.WoodQuality, f => new InquirySampleFaker().Generate().WoodQuality);
            RuleFor(f => f.ConstructionQuality, f => new InquirySampleFaker().Generate().ConstructionQuality);
            RuleFor(f => f.WoodCapacityHorizontalQuality, f => new InquirySampleFaker().Generate().WoodCapacityHorizontalQuality);
            RuleFor(f => f.PileWoodCapacityVerticalQuality, f => new InquirySampleFaker().Generate().PileWoodCapacityVerticalQuality);
            RuleFor(f => f.CarryingCapacityQuality, f => new InquirySampleFaker().Generate().CarryingCapacityQuality);
            RuleFor(f => f.MasonQuality, f => new InquirySampleFaker().Generate().MasonQuality);
            RuleFor(f => f.WoodQualityNecessity, f => new InquirySampleFaker().Generate().WoodQualityNecessity);
            RuleFor(f => f.CrackIndoorRestored, f => new InquirySampleFaker().Generate().CrackIndoorRestored);
            RuleFor(f => f.CrackIndoorType, f => new InquirySampleFaker().Generate().CrackIndoorType);
            RuleFor(f => f.CrackIndoorSize, f => new InquirySampleFaker().Generate().CrackIndoorSize);
            RuleFor(f => f.CrackFacadeFrontRestored, f => new InquirySampleFaker().Generate().CrackFacadeFrontRestored);
            RuleFor(f => f.CrackFacadeFrontType, f => new InquirySampleFaker().Generate().CrackFacadeFrontType);
            RuleFor(f => f.CrackFacadeFrontSize, f => new InquirySampleFaker().Generate().CrackFacadeFrontSize);
            RuleFor(f => f.CrackFacadeBackRestored, f => new InquirySampleFaker().Generate().CrackFacadeBackRestored);
            RuleFor(f => f.CrackFacadeBackType, f => new InquirySampleFaker().Generate().CrackFacadeBackType);
            RuleFor(f => f.CrackFacadeBackSize, f => new InquirySampleFaker().Generate().CrackFacadeBackSize);
            RuleFor(f => f.CrackFacadeLeftRestored, f => new InquirySampleFaker().Generate().CrackFacadeLeftRestored);
            RuleFor(f => f.CrackFacadeLeftType, f => new InquirySampleFaker().Generate().CrackFacadeLeftType);
            RuleFor(f => f.CrackFacadeLeftSize, f => new InquirySampleFaker().Generate().CrackFacadeLeftSize);
            RuleFor(f => f.CrackFacadeRightRestored, f => new InquirySampleFaker().Generate().CrackFacadeRightRestored);
            RuleFor(f => f.CrackFacadeRightType, f => new InquirySampleFaker().Generate().CrackFacadeRightType);
            RuleFor(f => f.CrackFacadeRightSize, f => new InquirySampleFaker().Generate().CrackFacadeRightSize);
            RuleFor(f => f.DeformedFacade, f => new InquirySampleFaker().Generate().DeformedFacade);
            RuleFor(f => f.ThresholdUpdownSkewed, f => new InquirySampleFaker().Generate().ThresholdUpdownSkewed);
            RuleFor(f => f.ThresholdFrontLevel, f => new InquirySampleFaker().Generate().ThresholdFrontLevel);
            RuleFor(f => f.ThresholdBackLevel, f => new InquirySampleFaker().Generate().ThresholdBackLevel);
            RuleFor(f => f.SkewedParallel, f => new InquirySampleFaker().Generate().SkewedParallel);
            RuleFor(f => f.SkewedPerpendicular, f => new InquirySampleFaker().Generate().SkewedPerpendicular);
            RuleFor(f => f.SkewedFacade, f => new InquirySampleFaker().Generate().SkewedFacade);
            RuleFor(f => f.SettlementSpeed, f => new InquirySampleFaker().Generate().SettlementSpeed);
        }
    }
}
