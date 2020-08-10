using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    /// Base class for building endpoint responses.
    /// </summary>
    public abstract class AnalysisWithStatisticsResponseBase<TRegion> : AnalysisResponseModelBase
        where TRegion : RegionResponseModel
    {
        /// <summary>
        /// Represents the region in which the additional statistics were calculated.
        /// </summary>
        public TRegion Region { get; set; }
    }
}
