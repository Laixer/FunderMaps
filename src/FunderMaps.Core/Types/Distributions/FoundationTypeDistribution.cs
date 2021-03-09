using System.Collections.Generic;

namespace FunderMaps.Core.Types.Distributions
{
    /// <summary>
    ///     Represents a distribution of foundation types.
    /// </summary>
    public sealed class FoundationTypeDistribution
    {
        /// <summary>
        ///     Contains a <see cref="FoundationTypePair"/> for each present foundation type.
        /// </summary>
        public IEnumerable<FoundationTypePair> FoundationTypes { get; set; }
    }
}
