using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels.Statistics
{
    /// <summary>
    /// Response wrapper for the foundation ratio statistics endpoint.
    /// </summary>
    public sealed class StatisticsFoundationRatioResponseModel : StatisticsResponseModelBase
    {
        /// <summary>
        /// Represents the distribution of foundation types.
        /// </summary>
        public FoundationTypeDistributionResponseModel FoundationTypeDistribution { get; set; }
    }
}
