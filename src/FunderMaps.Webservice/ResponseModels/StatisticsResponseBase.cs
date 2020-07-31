using FunderMaps.Webservice.ResponseModels.Types;
using FunderMaps.Webservice.Types;

namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    /// Base class for analysis endpoint responses.
    /// </summary>
    public abstract class StatisticsResponseModelBase : ResponseModelBase
    {
        /// <summary>
        /// Represents the region in which the statistics were calculated.
        /// </summary>
        public RegionResponseModel Region { get; set; }
    }
}
