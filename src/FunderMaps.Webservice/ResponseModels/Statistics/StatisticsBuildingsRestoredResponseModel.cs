namespace FunderMaps.Webservice.ResponseModels.Statistics
{
    /// <summary>
    ///     Response wrapper for buliding restoration statistics endpoint.
    /// </summary>
    public sealed class StatisticsBuildingsRestoredResponseModel : StatisticsResponseModelBase
    {
        /// <summary>
        ///     Total amount of restored buildings in the given area.
        /// </summary>
        public uint TotalBuildingRestoredCount { get; set; }
    }
}
