namespace FunderMaps.Webservice.ResponseModels.Statistics
{
    /// <summary>
    /// Response wrapper for the collected data statistics endpoint.
    /// </summary>
    public sealed class StatisticsDataCollectedResponseModel : StatisticsResponseModelBase
    {
        /// <summary>
        /// Represents the percentage of collected data in the given region.
        /// </summary>
        public double DataCollectedPercentage { get; set; }
    }
}
