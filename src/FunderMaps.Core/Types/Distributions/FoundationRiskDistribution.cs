namespace FunderMaps.Core.Types.Distributions
{
    /// <summary>
    /// Represents the distribution of foundation risks.
    /// </summary>
    public sealed class FoundationRiskDistribution
    {
        /// <summary>
        /// Percentage of foundations having risk A.
        /// </summary>
        public double PercentageA { get; set; }

        /// <summary>
        /// Percentage of foundations having risk B.
        /// </summary>
        public double PercentageB { get; set; }

        /// <summary>
        /// Percentage of foundations having risk C.
        /// </summary>
        public double PercentageC { get; set; }

        /// <summary>
        /// Percentage of foundations having risk D.
        /// </summary>
        public double PercentageD { get; set; }

        /// <summary>
        /// Percentage of foundations having risk E.
        /// </summary>
        public double PercentageE { get; set; }
    }
}
