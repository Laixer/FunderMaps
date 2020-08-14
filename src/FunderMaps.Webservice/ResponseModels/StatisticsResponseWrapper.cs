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
    }
}
