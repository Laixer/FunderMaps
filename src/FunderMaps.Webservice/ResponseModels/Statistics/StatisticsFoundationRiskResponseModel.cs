using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels.Statistics
{
    /// <summary>
    ///     Response wrapper for the foundation risk statistics endpoint.
    /// </summary>
    public sealed class StatisticsFoundationRiskResponseModel : StatisticsResponseModelBase
    {
        /// <summary>
        ///     Represents the distribution of foundation risks in the given region.
        /// </summary>
        public FoundationRiskDistributionResponseModel FoundationRiskDistribution { get; set; }
    }
}
