using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Webservice.Translation
{
    /// <summary>
    /// Contains translation functionality for <see cref="FoundationType"/>s.
    /// </summary>
    internal static class FoundationTypeTranslator
    {
        /// <summary>
        /// Translates a given <paramref name="foundationType"/> to it's corresponding
        /// string value.
        /// </summary>
        /// <param name="foundationType"><see cref="FoundationType"/></param>
        /// <returns><see cref="string"/> representation</returns>
        public static string Translate(FoundationType? foundationType)
            => foundationType switch
            {
                null => "null",
                FoundationType.Wood => "wood",
                FoundationType.WoodAmsterdam => "wood_amsterdam",
                FoundationType.WoodRotterdam => "wood_rotterdam",
                FoundationType.Concrete => "concrete",
                FoundationType.NoPile => "no_pile",
                FoundationType.NoPileMasonry => "no_pile_masonry",
                FoundationType.NoPileStrips => "no_pile_strips",
                FoundationType.NoPileBearingFloor => "no_pile_bearing_floor",
                FoundationType.NoPileConcreteFloor => "no_pile_concrete_floor",
                FoundationType.NoPileSlit => "no_pile_slit",
                FoundationType.WoodCharger => "wood_charger",
                FoundationType.WeightedPile => "weighted_pile",
                FoundationType.Combined => "combined",
                FoundationType.SteelPile => "steel_pile",
                FoundationType.Other => "other",
                FoundationType.Unknown => "unknown",
                _ => throw new InvalidOperationException(nameof(foundationType)),
            };
    }
}
