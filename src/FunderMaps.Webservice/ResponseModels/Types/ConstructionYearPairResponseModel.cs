namespace FunderMaps.Webservice.ResponseModels.Types
{
    /// <summary>
    ///     Response model representing how many buildings have their construction 
    ///     years in a given <see cref="Decade"/>.
    /// </summary>
    public sealed class ConstructionYearPairResponseModel
    {
        /// <summary>
        ///     Decade that represents this construction year pair.
        /// </summary>
        public YearsResponseModel Decade { get; set; }

        /// <summary>
        ///     Total amount of items that fall into this decade.
        /// </summary>
        public uint TotalCount { get; set; }
    }
}
