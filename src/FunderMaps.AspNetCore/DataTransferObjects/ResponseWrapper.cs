namespace FunderMaps.AspNetCore.DataTransferObjects;

/// <summary>
///     Base class for the response wrapper.
/// </summary>
public record ResponseWrapper { }

/// <summary>
///     Wrapper class for collections.
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
