using FunderMaps.Webservice.Types;

namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    /// Base class for building endpoint responses.
    /// </summary>
    public abstract class BuildingWithStatisticsResponseBase<TRegion> : BuildingResponseModelBase
        where TRegion : Region
    {
        /// <summary>
        /// Represents the region in which the additional statistics were calculated.
        /// </summary>
        public TRegion Region { get; set; }
    }
}
