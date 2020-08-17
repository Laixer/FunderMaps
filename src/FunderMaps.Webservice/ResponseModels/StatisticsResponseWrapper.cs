using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels
{
    /// TODO Do we need this?
    /// <summary>
    ///     Statistics response wrapper.
    /// </summary>
    /// <typeparam name="TResponseModel"><see cref="StatisticsResponseModelBase"/></typeparam>
    public class StatisticsResponseWrapper<TResponseModel> : ResponseWrapper<TResponseModel>
        where TResponseModel : StatisticsResponseModelBase
    {
        /// <summary>
        ///     Analysis product type.
        /// </summary>
        public StatisticsProductTypeResponseModel Product { get; set; }

        // FUTURE Do we want this?
        ///// <summary>
        /////     Represents the neighborhood code in which the statistics were calculated.
        /////     FUTURE Change to dynamic region.
        ///// </summary>
        //public string NeighborhoodCode { get; set; }

        // FUTURE Do we want this?
        ///// <summary>
        /////     Represents the neighborhood internal id in which the statistics were calculated.
        /////     FUTURE Change to dynamic region.
        ///// </summary>
        //public string NeighborhoodId { get; set; }
    }
}
