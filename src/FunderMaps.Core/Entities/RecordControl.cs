using System;

namespace FunderMaps.Core.Entities
{
    // TODO: Remove in the long term. This is meta data.
    /// <summary>
    ///     Record timestamps.
    /// </summary>
    public abstract class RecordControl<TEntity, TEntryIdentifier> : IdentifiableEntity<TEntity, TEntryIdentifier>
        where TEntity : class
        where TEntryIdentifier : IEquatable<TEntryIdentifier>, IComparable<TEntryIdentifier>
    {
        protected RecordControl(Func<TEntity, TEntryIdentifier> entryPrimaryKey)
            : base(entryPrimaryKey)
        {
        }

        /// <summary>
        ///     Record create date.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Record last update.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        ///     Record delete date.
        /// </summary>
        public DateTime? DeleteDate { get; set; }
    }
}
