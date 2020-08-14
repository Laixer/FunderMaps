using FunderMaps.Webservice.ResponseModels.Types;

namespace FunderMaps.Webservice.ResponseModels
{
    /// TODO Do we need this?
    /// <summary>
    ///     Response wrapper for analysis requests.
    /// </summary>
    /// <typeparam name="TResponseModel"><see cref="AnalysisResponseModelBase"/></typeparam>
    public class AnalysisResponseWrapper<TResponseModel> : ResponseWrapper<TResponseModel>
        where TResponseModel : AnalysisResponseModelBase
    {
        /// <summary>
        ///     Analysis product type.
        /// </summary>
        public AnalysisProductTypeResponseModel Product { get; set; }
    }
}
