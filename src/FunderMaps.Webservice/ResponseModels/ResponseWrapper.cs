using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    ///     Base class for the response wrapper. 
    /// </summary>
    /// <remarks>
    ///     This has no generics and thus can be used as a base for both abstract
    ///     and non-abstract implementations of the <see cref="ResponseWrapper{TResponseModel}"/>.
    /// </remarks>
    public class ResponseWrapper { }

    /// <summary>
    ///     Wrapper class for our API response object.
    /// </summary>
    /// <typeparam name="TResponseModel"><see cref="ResponseModelBase"/></typeparam>
    public class ResponseWrapper<TResponseModel> : ResponseWrapper
        where TResponseModel : ResponseModelBase
    {
        /// <summary>
        ///     Collection of <see cref="TResponseModel"/>.
        /// </summary>
        public IEnumerable<TResponseModel> Models { get; set; }

        /// <summary>
        ///     Total items in the <see cref="Models"/> field.
        /// </summary>
        public uint ModelCount => (uint)((Models == null) ? 0 : Models.Count());

        /// <summary>
        ///     Total items in the data store based on the request.
        /// </summary>
        public uint TotalCount { get; set; }
    }
}
