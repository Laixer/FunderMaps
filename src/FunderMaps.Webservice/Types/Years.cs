namespace FunderMaps.Webservice.Types
{
    /// <summary>
    /// Represents a collection of years.
    /// </summary>
    public sealed class Years
    {
        /// <summary>
        /// The first year for this collection of years.
        /// </summary>
        public Year YearFrom { get; set; }

        /// <summary>
        /// The last year in this collection of years.
        /// </summary>
        public Year YearTo { get; set; }
    }
}
