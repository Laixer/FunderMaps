using FunderMaps.Core.Types;
using FunderMaps.Webservice.ResponseModels.Types;
using System;

namespace FunderMaps.IntegrationTests.Webservice.MappingValidation
{
    /// <summary>
    ///     Contains explicit enum mapping.
    /// </summary>
    internal static class TestEnumMapper
    {
        internal static FoundationTypeResponseModel? Map(FoundationType? foundationType)
            => foundationType switch
            {
                null => null,
                FoundationType.Wood => FoundationTypeResponseModel.Wood,
                FoundationType.WoodAmsterdam => FoundationTypeResponseModel.WoodAmsterdam,
                FoundationType.WoodRotterdam => FoundationTypeResponseModel.WoodRotterdam,
                FoundationType.Concrete => FoundationTypeResponseModel.Concrete,
                FoundationType.NoPile => FoundationTypeResponseModel.NoPile,
                FoundationType.NoPileMasonry => FoundationTypeResponseModel.NoPileMasonry,
                FoundationType.NoPileStrips => FoundationTypeResponseModel.NoPileStrips,
                FoundationType.NoPileBearingFloor => FoundationTypeResponseModel.NoPileBearingFloor,
                FoundationType.NoPileConcreteFloor => FoundationTypeResponseModel.NoPileConcreteFloor,
                FoundationType.NoPileSlit => FoundationTypeResponseModel.NoPileSlit,
                FoundationType.WoodCharger => FoundationTypeResponseModel.WoodCharger,
                FoundationType.WeightedPile => FoundationTypeResponseModel.WeightedPile,
                FoundationType.Combined => FoundationTypeResponseModel.Combined,
                FoundationType.SteelPile => FoundationTypeResponseModel.SteelPile,
                FoundationType.Other => FoundationTypeResponseModel.Other,
                FoundationType.Unknown => FoundationTypeResponseModel.Unknown,
                _ => throw new InvalidOperationException(nameof(foundationType))
            };

        internal static FoundationRiskResponseModel? Map(FoundationRisk? foundationRisk)
            => foundationRisk switch
            {
                null => null,
                FoundationRisk.A => FoundationRiskResponseModel.A,
                FoundationRisk.B => FoundationRiskResponseModel.B,
                FoundationRisk.C => FoundationRiskResponseModel.C,
                FoundationRisk.D => FoundationRiskResponseModel.D,
                FoundationRisk.E => FoundationRiskResponseModel.E,
                _ => throw new InvalidOperationException(nameof(foundationRisk))
            };

        internal static ReliabilityResponseModel? Map(Reliability? reliability)
            => reliability switch
            {
                null => null,
                Reliability.Indicative => ReliabilityResponseModel.Indicative,
                Reliability.Established => ReliabilityResponseModel.Established,
                _ => throw new InvalidOperationException(nameof(reliability))
            };
    }
}
