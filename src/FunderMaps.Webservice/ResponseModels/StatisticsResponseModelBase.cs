using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    /// Base class for staistics endpoint responses.
    /// TODO Is this correct?
    /// </summary>
    public abstract class StatisticsResponseModelBase : ResponseModelBase
    {
        /// <summary>
        /// Represents the region in which the statistics were calculated.
        /// </summary>
        public RegionResponseModel Region { get; set; }
    }
}
