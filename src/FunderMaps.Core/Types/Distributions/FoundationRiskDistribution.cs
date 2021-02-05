namespace FunderMaps.Core.Types.Distributions
{
    /// <summary>
    ///     Represents the distribution of foundation risks.
    /// </summary>
    public sealed record FoundationRiskDistribution
    {
        /// <summary>
        ///     Percentage of foundations having risk A.
        /// </summary>
        public decimal PercentageA { get; set; }

        /// <summary>
        ///     Percentage of foundations having risk B.
        /// </summary>
        public decimal PercentageB { get; set; }

        /// <summary>
        ///     Percentage of foundations having risk C.
        /// </summary>
        public decimal PercentageC { get; set; }

        /// <summary>
        ///     Percentage of foundations having risk D.
        /// </summary>
        public decimal PercentageD { get; set; }

        /// <summary>
        ///     Percentage of foundations having risk E.
        /// </summary>
        public decimal PercentageE { get; set; }
    }
}
