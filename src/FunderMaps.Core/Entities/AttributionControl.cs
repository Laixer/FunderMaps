using System;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Attribution control.
    /// </summary>
    public abstract class AttributionControl<TEntity, TEntryIdentifier> : AccessControl<TEntity, TEntryIdentifier>
        where TEntity : class
        where TEntryIdentifier : IEquatable<TEntryIdentifier>, IComparable<TEntryIdentifier>
    {
        protected AttributionControl(Func<TEntity, TEntryIdentifier> entryPrimaryKey)
            : base(entryPrimaryKey)
        {
        }

        /// <summary>
        ///     Attribution key.
        /// </summary>
        public int Attribution { get; set; }

        /// <summary>
        ///     Attribution object.
        /// </summary>
        public Attribution AttributionNavigation { get; set; }
    }
}
