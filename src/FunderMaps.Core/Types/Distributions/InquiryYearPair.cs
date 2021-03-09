namespace FunderMaps.Core.Types.Distributions
{
    // FUTURE: Make pair a generic type.
    /// <summary>
    ///     Per year incident statistics.
    /// </summary>
    public sealed class InquiryYearPair
    {
        /// <summary>
        ///     Per year statistics.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///     Total amount of items that fall into this year.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
