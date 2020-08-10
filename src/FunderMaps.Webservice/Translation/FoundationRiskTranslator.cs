using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Webservice.Translation
{
    /// <summary>
    /// Contains translation functionality for <see cref="FoundationRisk"/>.
    /// </summary>
    internal static class FoundationRiskTranslator
    {
        /// <summary>
        /// Translates a <see cref="FoundationRisk"/> to the corresponding 
        /// string value.
        /// </summary>
        /// <param name="foundationRisk"><see cref="FoundationRisk"/></param>
        /// <returns><see cref="FoundationRisk"/> string value</returns>
        public static string Translate(FoundationRisk foundationRisk)
            => foundationRisk switch
            {
                FoundationRisk.A => "A",
                FoundationRisk.B => "B",
                FoundationRisk.C => "C",
                FoundationRisk.D => "D",
                FoundationRisk.E => "E",
                _ => throw new InvalidOperationException(nameof(foundationRisk))
            };
    }
}
