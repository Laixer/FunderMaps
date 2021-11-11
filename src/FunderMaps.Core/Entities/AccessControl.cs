using FunderMaps.Core.Types;
namespace FunderMaps.Core.Entities
{
    // FUTURE: Remove in the long term.
    /// <summary>
    ///     Record control.
    /// </summary>
    public abstract class AccessControl<TEntity, TEntryIdentifier> : RecordControl<TEntity, TEntryIdentifier>
        where TEntity : class
        where TEntryIdentifier : IEquatable<TEntryIdentifier>, IComparable<TEntryIdentifier>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        protected AccessControl(Func<TEntity, TEntryIdentifier> entryPrimaryKey)
            : base(entryPrimaryKey)
        {
        }

        /// <summary>
        ///     Record access policy.
        /// </summary>
        /// <remarks>Default to <see cref="AccessPolicy.Private"/>.</remarks>
        public AccessPolicy AccessPolicy { get; set; } = AccessPolicy.Private;

        /// <summary>
        ///     Is record public.
        /// </summary>
        /// <returns><c>True</c> if public.</returns>
        public bool IsPublic => AccessPolicy == AccessPolicy.Public;

        /// <summary>
        ///     Is record private.
        /// </summary>
        /// <returns><c>True</c> if private.</returns>
        public bool IsPrivate => AccessPolicy == AccessPolicy.Private;
    }
}
