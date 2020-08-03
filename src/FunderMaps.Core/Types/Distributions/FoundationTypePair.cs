namespace FunderMaps.Core.Types.Distributions
{
    /// <summary>
    /// Contains a pair of a <see cref="FoundationType"/> and the total amount
    /// of buildings having said <see cref="FoundationType"/>.
    /// </summary>
    public sealed class FoundationTypePair
    {
        /// <summary>
        /// The type of foundation.
        /// </summary>
        public FoundationType FoundationType { get; set; }

        /// <summary>
        /// The total amount of buildings having this foundation type.
        /// </summary>
        public uint TotalCount { get; set; }
    }
}
