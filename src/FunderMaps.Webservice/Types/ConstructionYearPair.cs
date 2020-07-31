namespace FunderMaps.Webservice.Types
{
    /// <summary>
    /// Represents how many buildings have their construction years in a given
    /// <see cref="Decade"/>.
    /// </summary>
    public sealed class ConstructionYearPair
    {
        /// <summary>
        /// Decade that represents this construction year pair.
        /// </summary>
        public Years Decade { get; set; }

        /// <summary>
        /// Total amount of items that fall into this decade.
        /// </summary>
        public uint TotalCount { get; set; }
    }
}
