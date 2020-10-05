namespace FunderMaps.Webservice.ResponseModels.Statistics
{
    /// <summary>
    ///     Response wrapper for the report statistics endpoint.
    /// </summary>
    public sealed class StatisticsReportsResponseModel : StatisticsResponseModelBase
    {
        /// <summary>
        ///     Total amount of reports in the given region.
        /// </summary>
        public uint TotalReportCount { get; set; }
    }
}
