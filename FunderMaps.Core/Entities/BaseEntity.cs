using System;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Base for all entities.
    /// </summary>
    /// <remarks>
    /// This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    /// Using non-generic integer types for simplicity and to ease caching logic.
    /// </remarks>
    public class BaseEntity
    {
    }
}
