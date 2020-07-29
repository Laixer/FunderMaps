using System.Collections.Generic;

namespace FunderMaps.Webservice.Types
{
    /// <summary>
    /// Represents a distribution of foundation types.
    /// </summary>
    public sealed class FoundationTypeDistribution
    {
        /// <summary>
        /// Contains a <see cref="FoundationTypePair"/> for each present foundation type.
        /// </summary>
        public IEnumerable<FoundationTypePair> FoundationTypes { get; set; }
    }
}
