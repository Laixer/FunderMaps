namespace FunderMaps.Webservice.ResponseModels.Statistics
{
    /// <summary>
    ///     Response wrapper for the incident statistics endpoint.
    /// </summary>
    public sealed class StatisticsIncidentsResponseModel : StatisticsResponseModelBase
    {
        /// <summary>
        ///     Total amount of incidents in the given region.
        /// </summary>
        public uint TotalIncidentCount { get; set; }
    }
}
