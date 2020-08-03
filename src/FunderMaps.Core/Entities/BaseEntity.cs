namespace FunderMaps.Core.Entities
{
    // TODO: IEquatable<T>, see https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1?redirectedfrom=MSDN&view=netcore-3.1

    /// <summary>
    ///     Base for all entities.
    /// </summary>
    /// <remarks>
    ///     This can easily be modified to be BaseEntity and public T Id to support different key types.
    ///     Using non-generic integer types for simplicity and to ease caching logic.
    /// </remarks>
    public abstract class BaseEntity
    {
        // TODO: Should be an abstract function.

        /// <summary>
        ///     Validate entity properties.
        /// </summary>
        public virtual void Validate()
        {
        }
    }
}
