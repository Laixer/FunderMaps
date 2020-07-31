using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels.Statistics
{
    /// <summary>
    /// Response wrapper for the building years ratio statistics endpoint.
    /// </summary>
    public sealed class StatisticsConstructionYearsResonseModel : StatisticsResponseModelBase
    {
        /// <summary>
        /// Represents the distribution of building construction years in the
        /// given region.
        /// </summary>
        public ConstructionYearDistributionResponseModel ConstructionYearDistribution { get; set; }
    }
}
