using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    /// Wrapper class for our API response object.
    /// </summary>
    /// <typeparam name="TResponseModel"><see cref="ModelBase"/></typeparam>
    public sealed class ResponseWrapper<TResponseModel>
        where TResponseModel : ResponseModelBase
    {
        /// <summary>
        /// String representation of the requested product.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Represents the type of a model.
        /// </summary>
        public string ModelType { get; set; }

        /// <summary>
        /// Collection of <see cref="TResponseModel"/>.
        /// </summary>
        public IEnumerable<TResponseModel> Models { get; set; }

        /// <summary>
        /// Total items in the <see cref="Models"/> field.
        /// </summary>
        public uint ModelCount => (uint)((Models == null) ? 0 : Models.Count());

        /// <summary>
        /// Total items in the data store based on the request.
        /// </summary>
        public uint TotalCount { get; set; }
    }
}
