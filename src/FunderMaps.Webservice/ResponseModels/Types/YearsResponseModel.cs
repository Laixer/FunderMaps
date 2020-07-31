namespace FunderMaps.Webservice.ResponseModels.Types
{
    /// <summary>
    /// Response models representing a range of <see cref="YearResponseModel"/>.
    /// </summary>
    public sealed class YearsResponseModel
    {
        /// <summary>
        /// The first year for this collection of years.
        /// </summary>
        public YearResponseModel YearFrom { get; set; }

        /// <summary>
        /// The last year in this collection of years.
        /// </summary>
        public YearResponseModel YearTo { get; set; }
    }
}
