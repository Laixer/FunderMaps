using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.Webservice.ResponseModels
{
    // TODO: Move to AspNetCore

    /// <summary>
    ///     Base class for the response wrapper.
    /// </summary>
    public record ResponseWrapper { }

    /// <summary>
    ///     Wrapper class for our API response object.
    /// </summary>
    public record ResponseWrapper<TDto> : ResponseWrapper
    {
        /// <summary>
        ///     Collection of <typeparamref name="TDto"/>.
        /// </summary>
        public IEnumerable<TDto> Items { get; init; }

        /// <summary>
        ///     Total items in the <see cref="Items"/> field.
        /// </summary>
        public int ItemCount => Items?.Count() ?? 0;
    }
}
