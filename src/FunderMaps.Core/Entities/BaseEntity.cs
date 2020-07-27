namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Base for all entities.
    /// </summary>
    /// <remarks>
    ///     This can easily be modified to be BaseEntity and public T Id to support different key types.
    ///     Using non-generic integer types for simplicity and to ease caching logic.
    /// </remarks>
    public abstract class BaseEntity
    {
        /// <summary>
        ///     Validate entity properties.
        /// </summary>
        public virtual void Validate()
        {
        }
    }
}
