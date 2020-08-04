namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    /// <see cref="ResponseWrapper{TResponseModel}"/> for multiple items.
    /// </summary>
    public sealed class ResponseMultipleWrapper<TResponseModel> : ResponseWrapper<TResponseModel>
        where TResponseModel : ResponseModelBase
    {
        /// <summary>
        /// Total items in the data store based on the request.
        /// </summary>
        public uint TotalCount { get; set; }
    }
}
